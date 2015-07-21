namespace SXN.Web
{
	/// <summary>
	/// Specifies the action to be performed on the resource which is specified by the URI.
	/// </summary>
	public enum HttpMethod
	{
		/// <summary>
		/// Invalid method.
		/// </summary>
		None,

		/// <summary>
		/// Requests to convert the Request connection to a transparent TCP/IP tunnel.
		/// </summary>
		Connect,

		/// <summary>
		/// Requests to delete the specified resource.
		/// </summary>
		Delete,

		/// <summary>
		/// Requests to return the representation of the specified resource.
		/// </summary>
		Get,

		/// <summary>
		/// Requests to return the response identical to the one that would correspond to a <see cref="Get"/> Request, but without the response body.
		/// </summary>
		Head,

		/// <summary>
		/// Requests to return the list of HTTP methods that are valid for the resource.
		/// </summary>
		Options,

		/// <summary>
		/// Requests to apply the partial modifications to the resource.
		/// </summary>
		Patch,

		/// <summary>
		/// Requests to create a new resource with the entity enclosed in the Request.
		/// </summary>
		Post,

		/// <summary>
		/// Requests to create a new resource or update the existing one with the entity enclosed in the Request.
		/// </summary>
		Put,

		/// <summary>
		/// Requests to echo back the received Request so that a client can see what (if any) changes or additions have been made by intermediate servers.
		/// </summary>
		Trace
	}
}