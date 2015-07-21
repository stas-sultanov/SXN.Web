using System;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SXN.Web
{
	/// <summary>
	/// Represents the base class for classes which handles the HTTP request.
	/// </summary>
	/// <typeparam name="TServer">The type of the HTTP server.</typeparam>
	public abstract class HttpRequestHandlerBase<TServer> : DisposableBase, IServerRequestHandler
		where TServer : HttpServerBase
	{
		#region Fields

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="HttpRequestHandlerBase{TServer}"/> class.
		/// </summary>
		/// <param name="server">The HTTP server which accepted the request.</param>
		/// <param name="context">An object that encapsulates information about the HTTP request.</param>
		/// <param name="acceptTime">The UTC time when request was accepted by the server.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected HttpRequestHandlerBase(TServer server, HttpContext context, DateTime acceptTime)
		{
			AcceptTime = acceptTime;

			Server = server;

			Context = context;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The UTC time when request was accepted by the server.
		/// </summary>
		public DateTime AcceptTime
		{
			get;
		}

		/// <summary>
		/// Gets the object that encapsulates information about the HTTP request.
		/// </summary>
		protected HttpContext Context
		{
			get;
		}

		/// <summary>
		/// The HTTP server which accepted the request.
		/// </summary>
		protected TServer Server
		{
			get;
		}

		#endregion

		#region Overrides of DisposableBase

		/// <summary>
		/// Releases managed resources held by class.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override sealed void ReleaseResources()
		{
			Context.Dispose();
		}

		#endregion

		#region Methods of IServerRequestHandler

		/// <summary>
		/// Initiates an asynchronous operation to try processes the HTTP request.
		/// </summary>
		/// <returns>
		/// A <see cref="Task{Boolean}"/> object of type <see cref="Boolean"/>> that represents the asynchronous operation.
		/// <see cref="Task{Boolean}.Result"/> equals <c>true</c> if operation has completed successfully, <c>false</c> otherwise.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public abstract Task<Boolean> TryProcessAsync();

		#endregion
	}
}