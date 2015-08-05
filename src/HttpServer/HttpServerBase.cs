using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace SXN.Web
{
	/// <summary>
	/// Provides a base class for the HTTP servers.
	/// </summary>
	public abstract class HttpServerBase : ServerBase
	{
		#region Fields

		/// <summary>
		/// The simple, programmatically controlled HTTP protocol listener.
		/// </summary>
		private readonly HttpListener httpListener;

		/// <summary>
		/// The collection of the types of the arguments of the constructor of the request handler.
		/// </summary>
		private readonly Type[] requestHandlerConstructorArgumentsTypes;

		/// <summary>
		/// The collection of the HTTP request patterns.
		/// </summary>
		private readonly IReadOnlyList<HttpRequestPattern> requestPatterns;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="HttpServerBase"/> class.
		/// </summary>
		/// <param name="settings">The configuration settings of the server.</param>
		/// <param name="diagnosticsEventHandler">A delegate to the method that will handle the diagnostics events.</param>
		/// <remarks>Constructor of the final class must set <see cref="WorkerBase.State"/> to the <see cref="EntityState.Inactive"/> state.</remarks>
		/// <exception cref="ArgumentException"><paramref name="settings"/> is <c>null</c> or is not valid.</exception>
		/// <exception cref="KeyNotFoundException"><paramref name="settings"/> does not contains the required performance counter configuration.</exception>
		protected internal HttpServerBase(EventHandler<DiagnosticsEventArgs> diagnosticsEventHandler, HttpServerSettings settings)
			: base(diagnosticsEventHandler, settings)
		{
			requestHandlerConstructorArgumentsTypes = new[]
			{
				GetType(), typeof(HttpContext), typeof(DateTime)
			};

			// Initialize a new instance of the HttpListener class
			httpListener = new HttpListener();

			// Get HttpListener configuration
			var httpListenerSettings = settings.Listener;

			// Add a Uniform Resource Identifier (URI) prefixes handled by this HttpListener object
			foreach (var prefix in httpListenerSettings.Prefixes)
			{
				httpListener.Prefixes.Add(prefix);
			}

			// Set the scheme used to authenticate clients
			httpListener.AuthenticationSchemes = httpListenerSettings.AuthenticationSchemes;

			// Set the realm associated with this HttpListener object
			httpListener.Realm = httpListenerSettings.Realm;

			// Specifies whether service receives exceptions that occur when an HttpListener sends the response to the client
			httpListener.IgnoreWriteExceptions = httpListenerSettings.IgnoreWriteExceptions;

			// Find HTTP routes
			requestPatterns = FindRequestsHandlers();
		}

		#endregion

		#region Overrides of ServerBase

		/// <summary>
		/// Performs operations required to deactivate server.
		/// </summary>
		protected override void ReleaseManagedResources()
		{
			// Shut down the HttpListener after processing all currently queued requests
			httpListener.Close();

			base.ReleaseManagedResources();
		}

		/// <summary>
		/// Initiates an asynchronous operation to try await the request.
		/// </summary>
		/// <returns>
		/// A <see cref="Task{IServerRequestHandler}"/> object of type <see cref="IServerRequestHandler"/> that represents the asynchronous operation.
		/// <see cref="Task{THandler}.Result"/> refers to the instance of <see cref="IServerRequestHandler"/> if operation was successful, <c>null</c> otherwise.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override sealed async Task<TryResult<IServerRequestHandler>> TryAwaitRequestAsync()
		{
			try
			{
				// Wait for the incoming request
				var listenerContext = await httpListener.GetContextAsync();

				// Get accept time
				var acceptTime = UtcNow;

				// Create context of the http request
				var httpContext = new HttpContext(listenerContext);

				// Check if request is not malformed
				if (!httpContext.Request.IsMalformed)
				{
					// Try find route
					return TryMatchRequestPattern(httpContext, acceptTime);
				}

				// Send back bad request
				httpContext.Response.SendBadRequest();
			}
			catch (HttpListenerException e)
			{
				TraceEvent(EventLevel.Error, e.ToString());
			}
			catch (InvalidOperationException e)
			{
				TraceEvent(EventLevel.Error, e.ToString());
			}

			return TryAwaitRequestFailResult;
		}

		#endregion

		#region Overrides of WorkerBase

		/// <summary>
		/// Initiates an asynchronous operation to activate server.
		/// </summary>
		/// <remarks>Requires server to be in <see cref="EntityState.Inactive"/> state.</remarks>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for a task to complete.</param>
		/// <returns>
		/// A <see cref="Task{Boolean}"/> object of type <see cref="Boolean"/>> that represents the asynchronous operation.
		/// <see cref="Task{Boolean}.Result"/> equals <c>true</c> if operation has completed successfully, <c>false</c> otherwise.
		/// </returns>
		protected override Task<Boolean> OnActivatingAsync(CancellationToken cancellationToken)
		{
			try
			{
				// Start HTTP listener
				httpListener.Start();
			}
			catch (HttpListenerException e)
			{
				TraceEvent(EventLevel.Critical, e.Message);

				return Task.FromResult(false);
			}
			catch (ObjectDisposedException e)
			{
				TraceEvent(EventLevel.Critical, e.Message);

				return Task.FromResult(false);
			}

			foreach (var prefix in httpListener.Prefixes)
			{
				TraceEvent(EventLevel.Informational, $"Is listening on {prefix}");
			}

			// Trace routes
			foreach (var requestPattern in requestPatterns)
			{
				TraceEvent(EventLevel.Informational, $"Is operating with request handler {requestPattern.Name}, has counter {requestPattern.HasCounter}");
			}

			return Task.FromResult(true);
		}

		/// <summary>
		/// Initiates an asynchronous operation to stop server.
		/// </summary>
		/// <remarks>Requires server to be in <see cref="EntityState.Active"/> state.</remarks>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for a task to complete.</param>
		/// <returns>A <see cref="Task"/> object that represents the asynchronous operation.</returns>
		protected override Task OnDeactivatingAsync(CancellationToken cancellationToken)
		{
			try
			{
				httpListener.Stop();
			}
			catch (ObjectDisposedException e)
			{
				TraceEvent(EventLevel.Error, e.ToString());
			}

			return Task.FromResult(0);
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Finds the request handlers within all loaded assemblies.
		/// </summary>
		/// <returns>List of rotes and handlers.</returns>
		private IReadOnlyList<HttpRequestPattern> FindRequestsHandlers()
		{
			// Select handlers types
			var typesRoutesAttributes =
				from
					// each assembly in domain
					assembly in AppDomain.CurrentDomain.GetAssemblies()
				from
					// each type in assembly
					type in assembly.GetTypes()
				where
					// type is final class derived from IRouteHandler
					typeof(IServerRequestHandler).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract
				let
					// get RouteAttribute
					routeAttribute = (HttpRequestPatternAttribute) Attribute.GetCustomAttribute(type, typeof(HttpRequestPatternAttribute))
				where
					routeAttribute != null
				orderby
					routeAttribute.Order
				select
					new
					{
						Type = type,
						RouteAttribute = routeAttribute
					};

			// Initialize result list
			var result = new List<HttpRequestPattern>();

			foreach (var item in typesRoutesAttributes)
			{
				// Get constructor of the handler
				var constructor = item.Type.GetConstructor(requestHandlerConstructorArgumentsTypes);

				if (constructor == null)
				{
					continue;
				}

				// Create construction delegate
				HttpRequestHandlerConstructor<HttpServerBase> constructorDelegate = delegate(HttpServerBase server, HttpContext httpContext, DateTime acceptTime)
				{
					var handler = constructor.Invoke(new Object[]
					{
						server, httpContext, acceptTime
					});

					return (IServerRequestHandler) handler;
				};

				// Get counter
				PerformanceCounter counter;

				PerformanceCounters.TryGetValue(item.RouteAttribute.Name, out counter);

				// Create route
				var route = new HttpRequestPattern(item.RouteAttribute.Method, item.RouteAttribute.UrlPattern, item.RouteAttribute.MaxContentLength, item.RouteAttribute.Name, constructorDelegate, counter);

				// Add to result
				result.Add(route);
			}

			return result.AsReadOnly();
		}

		/// <summary>
		/// Tries to find the HTTP request pattern that matches the HTTP request and create object that will handle the request.
		/// </summary>
		/// <param name="httpContext">An object that encapsulates all data about the HTTP request.</param>
		/// <param name="acceptTime">The UTC time when request was accepted by the server.</param>
		/// <returns>
		/// An instance of <see cref="TryResult{T}"/> which encapsulates result of the operation.
		/// <see cref="TryResult{T}.Success"/> contains <c>true</c> if operation was successful, <c>false</c> otherwise.
		/// <see cref="TryResult{T}.Result"/> contains valid object if operation was successful, <c>null</c> otherwise.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private TryResult<IServerRequestHandler> TryMatchRequestPattern(HttpContext httpContext, DateTime acceptTime)
		{
			var routesCount = requestPatterns.Count;

			// Look for the pattern
			for (var index = 0; index < routesCount; index++)
			{
				var route = requestPatterns[index];

				if (!route.TryMatch(httpContext.Request))
				{
					continue;
				}

				// Create handler
				var handler = route.CreateHandler(this, httpContext, acceptTime);

				return TryResult<IServerRequestHandler>.CreateSuccess(handler);
			}

			return TryAwaitRequestFailResult;
		}

		#endregion
	}
}