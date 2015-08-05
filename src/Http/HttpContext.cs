using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace SXN.Web
{
	/// <summary>
	/// Encapsulates information about the HTTP request.
	/// </summary>
	public sealed class HttpContext : IDisposable
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="HttpContext"/> class.
		/// </summary>
		internal HttpContext(HttpListenerContext context)
		{
			var listenerContext = context;

			Request = new HttpRequest(listenerContext.Request);

			Response = new HttpResponse(listenerContext.Response);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the <see cref="HttpRequest"/> object that represents a client's request to the server.
		/// </summary>
		public HttpRequest Request
		{
			get;
		}

		/// <summary>
		/// Gets the <see cref="HttpResponse"/> object that represents a data that will be sent to the client in response to the client's request.
		/// </summary>
		public HttpResponse Response
		{
			get;
		}

		#endregion

		#region Methods of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			Response.Dispose();
		}

		#endregion
	}
}