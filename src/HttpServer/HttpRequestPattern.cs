using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace SXN.Web
{
	/// <summary>
	/// Represents the pattern that the HTTP request must match to be handled.
	/// </summary>
	public sealed class HttpRequestPattern
	{
		#region Fields

		/// <summary>
		/// The performance counter which measures count of requests that matches this route.
		/// </summary>
		private readonly PerformanceCounter counter;

		/// <summary>
		/// The delegate to the method which creates handler that processes this route.
		/// </summary>
		private readonly HttpRequestHandlerConstructor<HttpServerBase> handlerConstructor;

		/// <summary>
		/// The maximal length of the content.
		/// </summary>
		private readonly Int64 maxContentLength;

		/// <summary>
		/// A minimal count of parameters within the query part of the URL.
		/// </summary>
		private readonly Int32 urlQueryMinArgsCount;

		/// <summary>
		/// The url segments.
		/// </summary>
		internal readonly IReadOnlyList<HttpUrlSegment> urlSegments;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="HttpRequestPattern"/> class.
		/// </summary>
		/// <param name="name">A name of the route.</param>
		/// <param name="method">An HTTP web method of the route.</param>
		/// <param name="urlPattern">The pattern of the url.</param>
		/// <param name="maxContentLength">A maximal length of the content.</param>
		/// <param name="handlerConstructor">A delegate to the method which creates handler that processes this route.</param>
		/// <param name="counter">A performance counter which measures count of requests that matches this route.</param>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="handlerConstructor"/> is <c>null</c>.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public HttpRequestPattern(HttpMethod method, String urlPattern, Int64 maxContentLength, String name, HttpRequestHandlerConstructor<HttpServerBase> handlerConstructor, PerformanceCounter counter)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			if (handlerConstructor == null)
			{
				throw new ArgumentNullException(nameof(handlerConstructor));
			}

			// Set fields

			Name = name;

			// Check delegate to the constructor of the handler
			this.handlerConstructor = handlerConstructor;

			Method = method;

			this.maxContentLength = maxContentLength;

			this.handlerConstructor = handlerConstructor;

			this.counter = counter;

			// Try parse arguments
			var templateTryParseResult = UrlArguments.TryParse(urlPattern);

			var segmentsTemp = new List<HttpUrlSegment>();

			foreach (var segment in templateTryParseResult.Result.Segments)
			{
				var match = Regex.Match(segment, @"\{(?<name>[a-zA-Z0-9-_]*)\}");

				if (match.Success)
				{
					segmentsTemp.Add(new HttpUrlSegment
					{
						IsVariable = true,
						Name = match.Value
					});
				}
				else
				{
					segmentsTemp.Add(new HttpUrlSegment
					{
						IsVariable = false,
						Name = segment
					});
				}
			}

			urlSegments = segmentsTemp.ToArray();

			urlQueryMinArgsCount = templateTryParseResult.Result.Query.Count;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the <see cref="Boolean"/> value that indicates whether pattern has associated performance counter.
		/// </summary>
		public Boolean HasCounter => counter != null;

		/// <summary>
		/// Gets the HTTP web method of the route.
		/// </summary>
		public HttpMethod Method
		{
			get;
		}

		/// <summary>
		/// Gets the name of the route.
		/// </summary>
		public String Name
		{
			get;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates a new object that will handle request to the HTTP server.
		/// </summary>
		/// <param name="server">The HTTP server which accepted the request.</param>
		/// <param name="context">An object that encapsulates information about the HTTP request.</param>
		/// <param name="acceptTime">The UTC time when request was accepted by the server.</param>
		/// <returns>A new instance of class derived from <see cref="IServerRequestHandler"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IServerRequestHandler CreateHandler<TWebServer>(TWebServer server, HttpContext context, DateTime acceptTime)
			where TWebServer : HttpServerBase
		{
			return handlerConstructor(server, context, acceptTime);
		}

		/// <summary>
		/// Tries to match the HTTP request to the current pattern.
		/// </summary>
		/// <param name="request">The HTTP request to match.</param>
		/// <returns><c>true</c> if request matches to the route,<c>false</c> otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean TryMatch(HttpRequest request)
		{
			// Check method
			if (Method != request?.Method)
			{
				return false;
			}

			var requestUrlSegments = request.UrlArguments.Segments;

			// Check URL segments count
			if (urlSegments.Count != requestUrlSegments.Count)
			{
				return false;
			}

			// Check URL query arguments count
			if (urlQueryMinArgsCount > request.UrlArguments.Query.Count)
			{
				return false;
			}

			// Check content length
			if (maxContentLength < request.ContentLength)
			{
				return false;
			}

			// Check segments
			// ReSharper disable once LoopCanBeConvertedToQuery
			for (var urlSegmentIndex = 0; urlSegmentIndex < urlSegments.Count; urlSegmentIndex++)
			{
				var urlSegment = urlSegments[urlSegmentIndex];

				if (urlSegment.IsVariable)
				{
					continue;
				}

				if (urlSegment.Name != requestUrlSegments[urlSegmentIndex])
				{
					return false;
				}
			}

			// Increment counter
			counter?.Increment();

			return true;
		}

		#endregion
	}
}