using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SXN.Web
{
	/// <summary>
	/// Provides a set of extension methods for the <see cref="HttpHeader"/> enumeration.
	/// </summary>
	public static class HttpHeaderEx
	{
		#region Constant and Static Fields

		/// <summary>
		/// Content-Types that are acceptable for the response.
		/// </summary>
		public const String Accept = @"Accept";

		/// <summary>
		/// Character sets that are acceptable.
		/// </summary>
		public const String AcceptCharset = @"Accept-Charset";

		/// <summary>
		/// Acceptable version in time.
		/// </summary>
		public const String AcceptDateTime = @"Accept-Datetime";

		/// <summary>
		/// List of acceptable encodings.
		/// </summary>
		public const String AcceptEncoding = @"Accept-Encoding";

		/// <summary>
		/// List of acceptable human languages for response.
		/// </summary>
		public const String AcceptLanguage = @"Accept-Language";

		/// <summary>
		/// Indicates whether or not the actual request can be made using credentials.
		/// </summary>
		public const String AccessControlAllowCredentials = @"Access-Control-Allow-Credentials";

		/// <summary>
		/// Indicates which HTTP headers can be used when making the actual request.
		/// </summary>
		public const String AccessControlAllowHeaders = @"Access-Control-Allow-Headers";

		/// <summary>
		/// Specifies the method or methods allowed when accessing the resource.
		/// </summary>
		public const String AccessControlAllowMethods = @"Access-Control-Allow-Methods";

		/// <summary>
		/// Specifies an URI that may access the resource.
		/// </summary>
		public const String AccessControlAllowOrigin = @"Access-Control-Allow-Origin";

		/// <summary>
		/// Specifies the white list of headers that client are allowed to access.
		/// </summary>
		public const String AccessControlExposeHeaders = @"Access-Control-Expose-Headers";

		/// <summary>
		/// Indicates how long the results of a preflight request can be cached.
		/// </summary>
		public const String AccessControlMaxAge = @"Access-Control-Max-Age";

		/// <summary>
		/// Used when issuing a preflight request to let the server know what HTTP headers will be used when the actual request is made.
		/// </summary>
		public const String AccessControlRequestHeaders = @"Access-Control-Request-Headers";

		/// <summary>
		/// Used when issuing a preflight request to let the server know what HTTP method will be used when the actual request is made.
		/// </summary>
		public const String AccessControlRequestMethod = @"Access-Control-Request-Method";

		/// <summary>
		/// Valid actions for a specified resource. To be used for a 405 Method not allowed
		/// </summary>
		public const String Allow = @"Allow";

		/// <summary>
		/// Authentication credentials for HTTP authentication.
		/// </summary>
		public const String Authorization = @"Authorization";

		/// <summary>
		/// Used to specify directives that MUST be obeyed by all caching mechanisms along the request/response chain.
		/// </summary>
		public const String CacheControl = @"Cache-Control";

		/// <summary>
		/// What type of connection the user-agent would prefer.
		/// </summary>
		public const String Connection = @"Connection";

		/// <summary>
		/// The type of encoding used on the Data. See HTTP compression.
		/// </summary>
		public const String ContentEncoding = @"Content-Encoding";

		/// <summary>
		/// The length of the request body in octets (8-bit bytes).
		/// </summary>
		public const String ContentLength = @"Content-Length";

		/// <summary>
		/// A Base64-encoded binary MD5 sum of the contentBuffer of the request body.
		/// </summary>
		public const String ContentMD5 = @"Content-MD5";

		/// <summary>
		/// The MIME type of the body of the request (used with POST and PUT requests).
		/// </summary>
		public const String ContentType = @"Content-Type";

		/// <summary>
		/// An HTTP cookie previously sent by the server with Set-Cookie.
		/// </summary>
		public const String Cookie = @"Cookie";

		/// <summary>
		/// The date and time that the message was sent.
		/// </summary>
		public const String Date = @"Date";

		/// <summary>
		/// Requests a web application to disable their tracking of a user.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public const String DNT = @"DNT";

		/// <summary>
		/// Indicates that particular server behaviors are required by the client.
		/// </summary>
		public const String Expect = @"Expect";

		/// <summary>
		/// The email address of the user making the request.
		/// </summary>
		public const String From = @"From";

		/// <summary>
		/// The domain name of the server (for virtual hosting), and the TCP port number on which the server is listening.
		/// </summary>
		public const String Host = @"Host";

		/// <summary>
		/// Only perform the action if the client supplied entity matches the same entity on the server.
		/// </summary>
		public const String IfMatch = @"If-Match";

		/// <summary>
		/// Allows a 304 Not Modified to be returned if contentBuffer is unchanged.
		/// </summary>
		public const String IfModifiedSince = @"If-Modified-Since";

		/// <summary>
		/// Allows a 304 Not Modified to be returned if contentBuffer is unchanged, see HTTP ETag.
		/// </summary>
		public const String IfNoneMatch = @"If-None-Match";

		/// <summary>
		/// If the entity is unchanged, send me the part(s) that I am missing; otherwise, send me the entire new entity.
		/// </summary>
		public const String IfRange = @"If-Range";

		/// <summary>
		/// Only send the response if the entity has not been modified since a specific time..
		/// </summary>
		public const String IfUnmodifiedSince = @"If-Unmodified-Since";

		/// <summary>
		/// Used in redirection, or when a new resource has been created.
		/// </summary>
		public const String Location = @"Location";

		/// <summary>
		/// Limit the number of times the message can be forwarded through proxies or gateways..
		/// </summary>
		public const String MaxForwards = @"Max-Forwards";

		/// <summary>
		/// Initiates a request for cross-origin resource sharing (asks server for an 'Access-Control-Allow-Origin' response header).
		/// </summary>
		public const String Origin = @"Origin";

		/// <summary>
		/// Implementation-specific headers that may have various effects anywhere along the request-response chain.
		/// </summary>
		public const String Pragma = @"Pragma";

		/// <summary>
		/// Authorization credentials for connecting to a proxy.
		/// </summary>
		public const String ProxyAuthorization = @"Proxy-Authorization";

		/// <summary>
		/// Has exactly the same functionality as standard <see cref="Connection"/> header.
		/// </summary>
		public const String ProxyConnection = @"Proxy-Connection";

		/// <summary>
		/// Request only part of an entity. Bytes are numbered from 0.
		/// </summary>
		public const String Range = @"Range";

		/// <summary>
		/// This is the address of the previous web page from which a link to the currently requested page was followed.
		/// </summary>
		public const String Referer = @"Referer";

		/// <summary>
		/// A name of the server.
		/// </summary>
		public const String Server = @"Server";

		/// <summary>
		/// An HTTP cookie.
		/// </summary>
		public const String SetCookie = @"Set-Cookie";

		/// <summary>
		/// The transfer encodings the user agent is willing to accept: the same values as for the response header Transfer-Encoding can be used, plus the "trailers" value (related to the "chunked" transfer method) to notify the server it expects to receive additional headers (the trailers) after the last, zero-sized, chunk.
		/// </summary>
		public const String TE = @"TE";

		/// <summary>
		/// Ask the server to upgrade to another protocol.
		/// </summary>
		public const String Upgrade = @"Upgrade";

		/// <summary>
		/// The user agent string of the user agent.
		/// </summary>
		public const String UserAgent = @"User-Agent";

		/// <summary>
		/// Informs the server of proxies through which the request was sent.
		/// </summary>
		public const String Via = @"Via";

		/// <summary>
		/// A general warning about possible problems with the entity body.
		/// </summary>
		public const String Warning = @"Warning";

		/// <summary>
		/// Allows easier parsing of the MakeModel/Firmware that is usually found in the User-Agent String of ATT Devices
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public const String XATTDeviceId = @"X-ATT-DeviceId";

		/// <summary>
		/// A standard for identifying the originating IP address of a client connecting to a web server through an HTTP proxy or load balancer
		/// </summary>
		public const String XForwardedFor = @"X-Forwarded-For";

		/// <summary>
		/// Identifying the originating protocol of an HTTP request.
		/// </summary>
		public const String XForwardedProto = @"X-Forwarded-Proto";

		/// <summary>
		/// Requests a web application override the method specified in the request.
		/// </summary>
		public const String XHttpMethodOverride = @"X-Http-Method-Override";

		/// <summary>
		/// Mainly used to identify Ajax requests.
		/// </summary>
		public const String XRequestedWith = @"X-Requested-With";

		/// <summary>
		/// Links to an XML file on the Internet with a full description and details about the device currently connecting.
		/// </summary>
		public const String XWapProfile = @"X-Wap-Profile";

		private static readonly TryResult<String> tryGetNameFailResult = TryResult<String>.CreateFail();

		private static readonly TryResult<HttpHeader> tryGetValueFailResult = TryResult<HttpHeader>.CreateFail();

		/// <summary>
		/// A dictionary of the HTTP header name and id pairs.
		/// </summary>
		private static readonly IReadOnlyDictionary<String, HttpHeader> namesIds = new Dictionary<String, HttpHeader>
		{
			{
				Accept, HttpHeader.Accept
			},
			{
				AcceptCharset, HttpHeader.AcceptCharset
			},
			{
				AcceptDateTime, HttpHeader.AcceptDateTime
			},
			{
				AcceptEncoding, HttpHeader.AcceptEncoding
			},
			{
				AcceptLanguage, HttpHeader.AcceptLanguage
			},
			{
				AccessControlRequestMethod, HttpHeader.AccessControlRequestMethod
			},
			{
				AccessControlRequestHeaders, HttpHeader.AccessControlRequestHeaders
			},
			{
				AccessControlAllowOrigin, HttpHeader.AccessControlAllowOrigin
			},
			{
				AccessControlAllowCredentials, HttpHeader.AccessControlAllowCredentials
			},
			{
				AccessControlExposeHeaders, HttpHeader.AccessControlExposeHeaders
			},
			{
				AccessControlMaxAge, HttpHeader.AccessControlMaxAge
			},
			{
				AccessControlAllowMethods, HttpHeader.AccessControlAllowMethods
			},
			{
				AccessControlAllowHeaders, HttpHeader.AccessControlAllowHeaders
			},
			{
				Allow, HttpHeader.Allow
			},
			{
				Authorization, HttpHeader.Authorization
			},
			{
				CacheControl, HttpHeader.CacheControl
			},
			{
				Connection, HttpHeader.Connection
			},
			{
				ContentEncoding, HttpHeader.ContentEncoding
			},
			{
				ContentLength, HttpHeader.ContentLength
			},
			{
				ContentMD5, HttpHeader.ContentMD5
			},
			{
				ContentType, HttpHeader.ContentType
			},
			{
				Cookie, HttpHeader.Cookie
			},
			{
				DNT, HttpHeader.DNT
			},
			{
				Date, HttpHeader.Date
			},
			{
				Expect, HttpHeader.Expect
			},
			{
				From, HttpHeader.From
			},
			{
				Host, HttpHeader.Host
			},
			{
				IfMatch, HttpHeader.IfMatch
			},
			{
				IfModifiedSince, HttpHeader.IfModifiedSince
			},
			{
				IfNoneMatch, HttpHeader.IfNoneMatch
			},
			{
				IfRange, HttpHeader.IfRange
			},
			{
				IfUnmodifiedSince, HttpHeader.IfUnmodifiedSince
			},
			{
				Location, HttpHeader.Location
			},
			{
				MaxForwards, HttpHeader.MaxForwards
			},
			{
				Origin, HttpHeader.Origin
			},
			{
				Pragma, HttpHeader.Pragma
			},
			{
				ProxyAuthorization, HttpHeader.ProxyAuthorization
			},
			{
				ProxyConnection, HttpHeader.ProxyConnection
			},
			{
				Range, HttpHeader.Range
			},
			{
				Referer, HttpHeader.Referer
			},
			{
				Server, HttpHeader.Server
			},
			{
				SetCookie, HttpHeader.SetCookie
			},
			{
				TE, HttpHeader.TE
			},
			{
				Upgrade, HttpHeader.Upgrade
			},
			{
				UserAgent, HttpHeader.UserAgent
			},
			{
				Via, HttpHeader.Via
			},
			{
				Warning, HttpHeader.Warning
			},
			{
				XATTDeviceId, HttpHeader.XATTDeviceId
			},
			{
				XForwardedFor, HttpHeader.XForwardedFor
			},
			{
				XForwardedProto, HttpHeader.XForwardedProto
			},
			{
				XHttpMethodOverride, HttpHeader.XHttpMethodOverride
			},
			{
				XRequestedWith, HttpHeader.XRequestedWith
			},
			{
				XWapProfile, HttpHeader.XWapProfile
			}
		};

		/// <summary>
		/// A dictionary of HTTP header id and name pairs.
		/// </summary>
		private static readonly IReadOnlyDictionary<HttpHeader, String> idsNames = new Dictionary<HttpHeader, String>
		{
			{
				HttpHeader.Accept, Accept
			},
			{
				HttpHeader.AcceptCharset, AcceptCharset
			},
			{
				HttpHeader.AcceptDateTime, AcceptDateTime
			},
			{
				HttpHeader.AcceptEncoding, AcceptEncoding
			},
			{
				HttpHeader.AcceptLanguage, AcceptLanguage
			},
			{
				HttpHeader.AccessControlRequestMethod, AccessControlRequestMethod
			},
			{
				HttpHeader.AccessControlRequestHeaders, AccessControlRequestHeaders
			},
			{
				HttpHeader.AccessControlAllowOrigin, AccessControlAllowOrigin
			},
			{
				HttpHeader.AccessControlAllowCredentials, AccessControlAllowCredentials
			},
			{
				HttpHeader.AccessControlExposeHeaders, AccessControlExposeHeaders
			},
			{
				HttpHeader.AccessControlMaxAge, AccessControlMaxAge
			},
			{
				HttpHeader.AccessControlAllowMethods, AccessControlAllowMethods
			},
			{
				HttpHeader.AccessControlAllowHeaders, AccessControlAllowHeaders
			},
			{
				HttpHeader.Allow, Allow
			},
			{
				HttpHeader.Authorization, Authorization
			},
			{
				HttpHeader.CacheControl, CacheControl
			},
			{
				HttpHeader.Connection, Connection
			},
			{
				HttpHeader.ContentEncoding, ContentEncoding
			},
			{
				HttpHeader.ContentLength, ContentLength
			},
			{
				HttpHeader.ContentMD5, ContentMD5
			},
			{
				HttpHeader.ContentType, ContentType
			},
			{
				HttpHeader.Cookie, Cookie
			},
			{
				HttpHeader.DNT, DNT
			},
			{
				HttpHeader.Date, Date
			},
			{
				HttpHeader.Expect, Expect
			},
			{
				HttpHeader.From, From
			},
			{
				HttpHeader.Host, Host
			},
			{
				HttpHeader.IfMatch, IfMatch
			},
			{
				HttpHeader.IfModifiedSince, IfModifiedSince
			},
			{
				HttpHeader.IfNoneMatch, IfNoneMatch
			},
			{
				HttpHeader.IfRange, IfRange
			},
			{
				HttpHeader.IfUnmodifiedSince, IfUnmodifiedSince
			},
			{
				HttpHeader.Location, Location
			},
			{
				HttpHeader.MaxForwards, MaxForwards
			},
			{
				HttpHeader.Origin, Origin
			},
			{
				HttpHeader.Pragma, Pragma
			},
			{
				HttpHeader.ProxyAuthorization, ProxyAuthorization
			},
			{
				HttpHeader.ProxyConnection, ProxyConnection
			},
			{
				HttpHeader.Range, Range
			},
			{
				HttpHeader.Referer, Referer
			},
			{
				HttpHeader.Server, Server
			},
			{
				HttpHeader.SetCookie, SetCookie
			},
			{
				HttpHeader.TE, TE
			},
			{
				HttpHeader.Upgrade, Upgrade
			},
			{
				HttpHeader.UserAgent, UserAgent
			},
			{
				HttpHeader.Via, Via
			},
			{
				HttpHeader.Warning, Warning
			},
			{
				HttpHeader.XATTDeviceId, XATTDeviceId
			},
			{
				HttpHeader.XForwardedFor, XForwardedFor
			},
			{
				HttpHeader.XForwardedProto, XForwardedProto
			},
			{
				HttpHeader.XHttpMethodOverride, XHttpMethodOverride
			},
			{
				HttpHeader.XRequestedWith, XRequestedWith
			},
			{
				HttpHeader.XWapProfile, XWapProfile
			}
		};

		#endregion

		#region Methods

		/// <summary>
		/// Tries to convert the representation of the HTTP header from string to integer.
		/// </summary>
		/// <param name="name">The string representation of the HTTP header.</param>
		/// <param name="id">Contains the integer representation of the HTTP header if operation was successful, <see cref="HttpHeader.None"/> otherwise.</param>
		/// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean TryGetId(this String name, out HttpHeader id)
		{
			return namesIds.TryGetValue(name, out id);
		}

		/// <summary>
		/// Tries to convert the representation of the HTTP header from string to integer.
		/// </summary>
		/// <param name="name">The string representation of the HTTP header.</param>
		/// <returns>
		/// An instance of <see cref="TryResult{T}"/> which encapsulates result of the operation.
		/// <see cref="TryResult{T}.Success"/> contains <c>true</c> if operation was successful, <c>false</c> otherwise.
		/// <see cref="TryResult{T}.Result"/> contains the integer representation of the HTTP header if operation was successful, <see cref="HttpHeader.None"/> otherwise.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TryResult<HttpHeader> TryGetId(this String name)
		{
			if (name == null)
			{
				return tryGetValueFailResult;
			}

			HttpHeader result;

			return namesIds.TryGetValue(name, out result) ? TryResult<HttpHeader>.CreateSuccess(result) : tryGetValueFailResult;
		}

		/// <summary>
		/// Tries to convert the representation of the HTTP header from integer to string.
		/// </summary>
		/// <param name="id">The integer representation of the HTTP header.</param>
		/// <param name="name">Contains the string representation of the HTTP header if operation was successful, <c>null</c> otherwise.</param>
		/// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean TryGetName(this HttpHeader id, out String name)
		{
			return idsNames.TryGetValue(id, out name);
		}

		/// <summary>
		/// Tries to convert the representation of the HTTP header from integer to string.
		/// </summary>
		/// <param name="id">The integer representation of the HTTP header.</param>
		/// <returns>
		/// An instance of <see cref="TryResult{T}"/> which encapsulates result of the operation.
		/// <see cref="TryResult{T}.Success"/> contains <c>true</c> if operation was successful, <c>false</c> otherwise.
		/// <see cref="TryResult{T}.Result"/> contains the string representation of the HTTP header if operation was successful, <c>null</c> otherwise.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TryResult<String> TryGetName(this HttpHeader id)
		{
			String result;

			return idsNames.TryGetValue(id, out result) ? TryResult<String>.CreateSuccess(result) : tryGetNameFailResult;
		}

		#endregion
	}
}