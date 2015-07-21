namespace SXN.Web
{
	/// <summary>
	/// Contains the values of status codes defined for HTTP.
	/// </summary>
	public enum HttpStatusCode : ushort
	{
		None = 0,

		/// <summary>
		/// Indicates that the client can continue with its request.
		/// </summary>
		Continue = 100,

		/// <summary>
		/// Indicates that client requests to change the protocol version or protocol.
		/// </summary>
		SwitchingProtocols = 101,

		/// <summary>
		/// Indicates that the request succeeded and that the requested information is in the response.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		OK = 200,

		/// <summary>
		/// Indicates that the request resulted in a new resource created before the response was sent.
		/// </summary>
		Created = 201,

		/// <summary>
		/// Indicates that the request has been accepted for further processing.
		/// </summary>
		Accepted = 202,

		/// <summary>
		/// Indicates that the client has indicated with <see cref="HttpHeader.Accept"/> headers that it will not accept any of the available representations of the resource.
		/// </summary>
		NonAuthoritativeInformation = 203,

		/// <summary>
		/// Indicates that the request has been successfully processed and that the response is intentionally blank.
		/// </summary>
		NoContent = 204,

		/// <summary>
		/// Indicates that the client should reset the current resource.
		/// </summary>
		ResetContent = 205,

		/// <summary>
		/// Indicates that the response is a partial response as requested by a <see cref="HttpMethod.Get"/> request that includes a byte range.
		/// </summary>
		PartialContent = 206,

		/// <summary>
		/// Indicates that the requested information has multiple representations.
		/// </summary>
		MultipleChoices = 300,

		/// <summary>
		/// Indicates that the requested information has been moved to the URI specified in the <see cref="HttpHeader.Location"/> header.
		/// </summary>
		MovedPermanently = 301,

		/// <summary>
		/// Indicates that the requested information is located at the URI specified in the <see cref="HttpHeader.Location"/> header.
		/// </summary>
		Found = 302,

		/// <summary>
		/// Indicates that the response to the request can be found under another URI using a <see cref="HttpMethod.Get"/> method.
		/// </summary>
		SeeOther = 303,

		/// <summary>
		/// Indicates that the client's cached copy is up to date.
		/// </summary>
		NotModified = 304,

		/// <summary>
		/// Indicates that the request should use the proxy server at the URI specified in the <see cref="HttpHeader.Location"/> header.
		/// </summary>
		UseProxy = 305,

		/// <summary>
		/// Indicates that the request information is located at the URI specified in the <see cref="HttpHeader.Location"/> header.
		/// </summary>
		TemporaryRedirect = 307,

		/// <summary>
		/// Indicates that the request could not be understood by the server.
		/// </summary>
		BadRequest = 400,

		/// <summary>
		/// Indicates that the requested resource requires authentication.
		/// </summary>
		Unauthorized = 401,

		/// <summary>
		/// Indicates that the requested resource requires payment.
		/// </summary>
		PaymentRequired = 402,

		/// <summary>
		/// Indicates that the server refuses to fulfill the request.
		/// </summary>
		Forbidden = 403,

		/// <summary>
		/// Indicates that the requested resource does not exist on the server.
		/// </summary>
		NotFound = 404,

		/// <summary>
		/// Indicates that the request method is not allowed on the requested resource.
		/// </summary>
		MethodNotAllowed = 405,

		/// <summary>
		/// Indicates that the client has indicated with <see cref="HttpHeader.Accept"/> headers that it will not accept any of the available representations of the resource.
		/// </summary>
		NotAcceptable = 406,

		/// <summary>
		/// Indicates that the requested proxy requires authentication.
		/// </summary>
		ProxyAuthenticationRequired = 407,

		/// <summary>
		/// Indicates that the client did not send a request within the time the server was expecting the request.
		/// </summary>
		RequestTimeout = 408,

		/// <summary>
		/// Indicates that the request could not be carried out because of a conflict on the server.
		/// </summary>
		Conflict = 409,

		/// <summary>
		/// Indicates that the requested resource is no longer available.
		/// </summary>
		Gone = 410,

		/// <summary>
		/// Indicates that the required <see cref="HttpHeader.ContentLength"/> header is missing.
		/// </summary>
		LengthRequired = 411,

		/// <summary>
		/// Indicates that a condition set for this request failed, and the request cannot be carried out.
		/// </summary>
		PreconditionFailed = 412,

		/// <summary>
		/// Indicates that the request is too large for the server to process.
		/// </summary>
		RequestEntityTooLarge = 413,

		/// <summary>
		/// Indicates that the URI is too long.
		/// </summary>
		RequestUriTooLong = 414,

		/// <summary>
		/// Indicates that the request is an unsupported type.
		/// </summary>
		UnsupportedMediaType = 415,

		/// <summary>
		/// Indicates that the range of data requested from the resource cannot be returned.
		/// </summary>
		RequestedRangeNotSatisfiable = 416,

		/// <summary>
		/// Indicates that an expectation given in an <see cref="HttpHeader.Expect"/> header could not be met by the server.
		/// </summary>
		ExpectationFailed = 417,

		/// <summary>
		/// Indicates that the client should switch to a different protocol.
		/// </summary>
		UpgradeRequired = 426,

		/// <summary>
		/// Indicates that a generic error has occurred on the server.
		/// </summary>
		InternalServerError = 500,

		/// <summary>
		/// Indicates that the server does not support the requested function.
		/// </summary>
		NotImplemented = 501,

		/// <summary>
		/// Indicates that an intermediate proxy server received a bad response from another proxy or the origin server.
		/// </summary>
		BadGateway = 502,

		/// <summary>
		/// Indicates that the server is temporarily unavailable, usually due to high load or maintenance.
		/// </summary>
		ServiceUnavailable = 503,

		/// <summary>
		/// Indicates that an intermediate proxy server timed out while waiting for a response from another proxy or the origin server.
		/// </summary>
		GatewayTimeout = 504,

		/// <summary>
		/// Indicates that the requested HTTP version is not supported by the server.
		/// </summary>
		HttpVersionNotSupported = 505
	}
}