namespace SXN.Web
{
	/// <summary>
	/// Represents the <a href="http://tools.ietf.org/html/rfc7231">standard</a> HTTP header.
	/// </summary>
	public enum HttpHeader
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,

		/// <summary>
		/// Content-Types that are acceptable for the response.
		/// </summary>
		Accept = 0x41000100,

		/// <summary>
		/// Character sets that are acceptable.
		/// </summary>
		AcceptCharset = 0x41000200,

		/// <summary>
		/// Acceptable version in time.
		/// </summary>
		AcceptDateTime = 0x41000300,

		/// <summary>
		/// List of acceptable encodings.
		/// </summary>
		AcceptEncoding = 0x41000400,

		/// <summary>
		/// List of acceptable human languages for response.
		/// </summary>
		AcceptLanguage = 0x41000500,

		/// <summary>
		/// Indicates whether or not the actual request can be made using credentials.
		/// </summary>
		AccessControlAllowCredentials = 0x41000601,

		/// <summary>
		/// Indicates which HTTP headers can be used when making the actual request.
		/// </summary>
		AccessControlAllowHeaders = 0x41000602,

		/// <summary>
		/// Specifies the method or methods allowed when accessing the resource.
		/// </summary>
		AccessControlAllowMethods = 0x41000603,

		/// <summary>
		/// Specifies an URI that may access the resource.
		/// </summary>
		AccessControlAllowOrigin = 0x41000604,

		/// <summary>
		/// Specifies the white list of headers that client are allowed to access.
		/// </summary>
		AccessControlExposeHeaders = 0x41000605,

		/// <summary>
		/// Indicates how long the results of a preflight request can be cached.
		/// </summary>
		AccessControlMaxAge = 0x41000606,

		/// <summary>
		/// Used when issuing a preflight request to let the server know what HTTP headers will be used when the actual request is made.
		/// </summary>
		AccessControlRequestHeaders = 0x41000607,

		/// <summary>
		/// Used when issuing a preflight request to let the server know what HTTP method will be used when the actual request is made.
		/// </summary>
		AccessControlRequestMethod = 0x41000608,

		/// <summary>
		/// Valid actions for a specified resource. To be used for a 405 Method not allowed
		/// </summary>
		Allow = 0x41000700,

		/// <summary>
		/// Authentication credentials for HTTP authentication.
		/// </summary>
		Authorization = 0x41000800,

		/// <summary>
		/// Used to specify directives that MUST be obeyed by all caching mechanisms along the request/response chain.
		/// </summary>
		CacheControl = 0x43000100,

		/// <summary>
		/// What type of connection the user-agent would prefer.
		/// </summary>
		Connection = 0x43000200,

		/// <summary>
		/// The type of encoding used on the Data. See HTTP compression.
		/// </summary>
		ContentEncoding = 0x43000300,

		/// <summary>
		/// The length of the request body in octets (8-bit bytes).
		/// </summary>
		ContentLength = 0x43000400,

		/// <summary>
		/// A Base64-encoded binary MD5 sum of the contentBuffer of the request body.
		/// </summary>
		ContentMD5 = 0x43000500,

		/// <summary>
		/// The MIME type of the body of the request (used with POST and PUT requests).
		/// </summary>
		ContentType = 0x43000600,

		/// <summary>
		/// An HTTP cookie previously sent by the server with Set-Cookie.
		/// </summary>
		Cookie = 0x43000700,

		/// <summary>
		/// The date and time that the message was sent.
		/// </summary>
		Date = 0x44000100,

		/// <summary>
		/// Requests a web application to disable their tracking of a user.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		DNT = 0x44000200,

		/// <summary>
		/// Indicates that particular server behaviors are required by the client.
		/// </summary>
		Expect = 0x45000100,

		/// <summary>
		/// The email address of the user making the request.
		/// </summary>
		From = 0x46000100,

		/// <summary>
		/// The domain name of the server (for virtual hosting), and the TCP port number on which the server is listening.
		/// </summary>
		Host = 0x48000100,

		/// <summary>
		/// Only perform the action if the client supplied entity matches the same entity on the server.
		/// </summary>
		IfMatch = 0x49000100,

		/// <summary>
		/// Allows a 304 Not Modified to be returned if contentBuffer is unchanged.
		/// </summary>
		IfModifiedSince = 0x49000200,

		/// <summary>
		/// Allows a 304 Not Modified to be returned if contentBuffer is unchanged, see HTTP ETag.
		/// </summary>
		IfNoneMatch = 0x49000300,

		/// <summary>
		/// If the entity is unchanged, send me the part(s) that I am missing; otherwise, send me the entire new entity.
		/// </summary>
		IfRange = 0x49000400,

		/// <summary>
		/// Only send the response if the entity has not been modified since a specific time..
		/// </summary>
		IfUnmodifiedSince = 0x49000500,

		/// <summary>
		/// Used in redirection, or when a new resource has been created.
		/// </summary>
		Location = 0x4C000100,

		/// <summary>
		/// Limit the number of times the message can be forwarded through proxies or gateways..
		/// </summary>
		MaxForwards = 0x4D000100,

		/// <summary>
		/// Initiates a request for cross-origin resource sharing (asks server for an 'Access-Control-Allow-Origin' response header).
		/// </summary>
		Origin = 0x4F000100,

		/// <summary>
		/// Implementation-specific headers that may have various effects anywhere along the request-response chain.
		/// </summary>
		Pragma = 0x50000100,

		/// <summary>
		/// Authorization credentials for connecting to a proxy.
		/// </summary>
		ProxyAuthorization = 0x50000200,

		/// <summary>
		/// Has exactly the same functionality as standard <see cref="Connection"/> header.
		/// </summary>
		ProxyConnection = 0x50000300,

		/// <summary>
		/// Request only part of an entity. Bytes are numbered from 0.
		/// </summary>
		Range = 0x52000100,

		/// <summary>
		/// This is the address of the previous web page from which a link to the currently requested page was followed.
		/// </summary>
		Referer = 0x52000200,

		/// <summary>
		/// A name of the server.
		/// </summary>
		Server = 0x53000100,

		/// <summary>
		/// An HTTP cookie.
		/// </summary>
		SetCookie = 0x53000200,

		/// <summary>
		/// The transfer encodings the user agent is willing to accept: the same values as for the response header Transfer-Encoding can be used, plus the "trailers" value (related to the "chunked" transfer method) to notify the server it expects to receive additional headers (the trailers) after the last, zero-sized, chunk.
		/// </summary>
		TE = 0x54000100,

		/// <summary>
		/// Ask the server to upgrade to another protocol.
		/// </summary>
		Upgrade = 0x55000100,

		/// <summary>
		/// The user agent string of the user agent.
		/// </summary>
		UserAgent = 0x55000200,

		/// <summary>
		/// Informs the server of proxies through which the request was sent.
		/// </summary>
		Via = 0x56000100,

		/// <summary>
		/// A general warning about possible problems with the entity body.
		/// </summary>
		Warning = 0x57000100,

		/// <summary>
		/// Allows easier parsing of the MakeModel/Firmware that is usually found in the User-Agent String of ATT Devices
		/// </summary>
		// ReSharper disable once InconsistentNaming
		XATTDeviceId = 0x58000100,

		/// <summary>
		/// A standard for identifying the originating IP address of a client connecting to a web server through an HTTP proxy or load balancer
		/// </summary>
		XForwardedFor = 0x58000200,

		/// <summary>
		/// Identifying the originating protocol of an HTTP request.
		/// </summary>
		XForwardedProto = 0x58000300,

		/// <summary>
		/// Requests a web application override the method specified in the request.
		/// </summary>
		XHttpMethodOverride = 0x58000400,

		/// <summary>
		/// Mainly used to identify Ajax requests.
		/// </summary>
		XRequestedWith = 0x58000500,

		/// <summary>
		/// Links to an XML file on the Internet with a full description and details about the device currently connecting.
		/// </summary>
		XWapProfile = 0x58000600
	}
}