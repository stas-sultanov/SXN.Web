using System;
using System.ServiceModel;

namespace SXN.Web
{
	/// <summary>
	/// Creates a new object that will handle request to the HTTP server.
	/// </summary>
	/// <param name="server">The HTTP server which accepted the request.</param>
	/// <param name="context">An object that encapsulates information about the HTTP request.</param>
	/// <param name="acceptTime"> The UTC time when request was accepted by the server.</param>
	/// <returns>A new instance of class derived from <see cref="IServerRequestHandler"/></returns>
	public delegate IServerRequestHandler HttpRequestHandlerConstructor<in TWebServer>(TWebServer server, HttpContext context, DateTime acceptTime)
		where TWebServer : HttpServerBase;
}