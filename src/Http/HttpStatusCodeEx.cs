using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SXN.Web
{
	/// <summary>
	/// Provides a set of extension methods for the <see cref="HttpStatusCode"/> enumeration.
	/// </summary>
	public static class HttpStatusCodeEx
	{
		#region Constant and Static Fields

		private static readonly TryResult<String> getDescriptionFailResult = TryResult<String>.CreateFail();

		private static readonly IReadOnlyDictionary<HttpStatusCode, TryResult<String>> httpStatusDescriptions = new ReadOnlyDictionary<HttpStatusCode, TryResult<String>>(new Dictionary<HttpStatusCode, TryResult<String>>
		{
			// 1xx: Informational
			{
				HttpStatusCode.Continue, TryResult<String>.CreateSuccess(@"Continue")
			},
			{
				HttpStatusCode.SwitchingProtocols, TryResult<String>.CreateSuccess(@"Switching Protocols")
			},

			// 2xx: Success
			{
				HttpStatusCode.OK, TryResult<String>.CreateSuccess(@"OK")
			},
			{
				HttpStatusCode.Created, TryResult<String>.CreateSuccess(@"Created")
			},
			{
				HttpStatusCode.Accepted, TryResult<String>.CreateSuccess(@"Accepted")
			},
			{
				HttpStatusCode.NonAuthoritativeInformation, TryResult<String>.CreateSuccess(@"Non-Authoritative Information")
			},
			{
				HttpStatusCode.NoContent, TryResult<String>.CreateSuccess(@"No Content")
			},
			{
				HttpStatusCode.ResetContent, TryResult<String>.CreateSuccess(@"Reset Content")
			},
			{
				HttpStatusCode.PartialContent, TryResult<String>.CreateSuccess(@"Partial Content")
			},

			// 3xx: Redirection
			{
				HttpStatusCode.MultipleChoices, TryResult<String>.CreateSuccess(@"Multiple Choices")
			},
			{
				HttpStatusCode.MovedPermanently, TryResult<String>.CreateSuccess(@"Moved Permanently")
			},
			{
				HttpStatusCode.Found, TryResult<String>.CreateSuccess(@"Found")
			},
			{
				HttpStatusCode.SeeOther, TryResult<String>.CreateSuccess(@"See Other")
			},
			{
				HttpStatusCode.NotModified, TryResult<String>.CreateSuccess(@"Not Modified")
			},
			{
				HttpStatusCode.UseProxy, TryResult<String>.CreateSuccess(@"Use Proxy")
			},
			{
				HttpStatusCode.TemporaryRedirect, TryResult<String>.CreateSuccess(@"Temporary Redirect")
			},

			// 4xx: Client Error
			{
				HttpStatusCode.BadRequest, TryResult<String>.CreateSuccess(@"Bad Request")
			},
			{
				HttpStatusCode.Unauthorized, TryResult<String>.CreateSuccess(@"Unauthorized")
			},
			{
				HttpStatusCode.PaymentRequired, TryResult<String>.CreateSuccess(@"Payment Required")
			},
			{
				HttpStatusCode.Forbidden, TryResult<String>.CreateSuccess(@"Forbidden")
			},
			{
				HttpStatusCode.NotFound, TryResult<String>.CreateSuccess(@"Not Found")
			},
			{
				HttpStatusCode.MethodNotAllowed, TryResult<String>.CreateSuccess(@"Method Not Allowed")
			},
			{
				HttpStatusCode.NotAcceptable, TryResult<String>.CreateSuccess(@"Not Acceptable")
			},
			{
				HttpStatusCode.ProxyAuthenticationRequired, TryResult<String>.CreateSuccess(@"Proxy Authentication Required")
			},
			{
				HttpStatusCode.RequestTimeout, TryResult<String>.CreateSuccess(@"Request Timeout")
			},
			{
				HttpStatusCode.Conflict, TryResult<String>.CreateSuccess(@"Conflict")
			},
			{
				HttpStatusCode.Gone, TryResult<String>.CreateSuccess(@"Gone")
			},
			{
				HttpStatusCode.LengthRequired, TryResult<String>.CreateSuccess(@"Length Required")
			},
			{
				HttpStatusCode.PreconditionFailed, TryResult<String>.CreateSuccess(@"Precondition Failed")
			},
			{
				HttpStatusCode.RequestEntityTooLarge, TryResult<String>.CreateSuccess(@"Request Entity Too Large")
			},
			{
				HttpStatusCode.RequestUriTooLong, TryResult<String>.CreateSuccess(@"Request Uri Too Long")
			},
			{
				HttpStatusCode.UnsupportedMediaType, TryResult<String>.CreateSuccess(@"Unsupported Media Type")
			},
			{
				HttpStatusCode.RequestedRangeNotSatisfiable, TryResult<String>.CreateSuccess(@"Requested Range Not Satisfiable")
			},
			{
				HttpStatusCode.ExpectationFailed, TryResult<String>.CreateSuccess(@"Expectation Failed")
			},
			{
				HttpStatusCode.UpgradeRequired, TryResult<String>.CreateSuccess(@"Upgrade Required")
			},

			// 5xx: Server Error
			{
				HttpStatusCode.InternalServerError, TryResult<String>.CreateSuccess(@"Internal Server Error")
			},
			{
				HttpStatusCode.NotImplemented, TryResult<String>.CreateSuccess(@"Not Implemented")
			},
			{
				HttpStatusCode.BadGateway, TryResult<String>.CreateSuccess(@"Bad Gateway")
			},
			{
				HttpStatusCode.ServiceUnavailable, TryResult<String>.CreateSuccess(@"Service Unavailable")
			},
			{
				HttpStatusCode.GatewayTimeout, TryResult<String>.CreateSuccess(@"Gateway Timeout")
			},
			{
				HttpStatusCode.HttpVersionNotSupported, TryResult<String>.CreateSuccess(@"HTTP Version Not Supported")
			}
		});

		#endregion

		#region Methods

		/// <summary>
		/// Tries to gets the description of the HTTP status code.
		/// </summary>
		/// <returns>
		/// An instance of <see cref="TryResult{T}"/> which encapsulates result of the operation.
		/// <see cref="TryResult{T}.Success"/> contains <c>true</c> if operation was successful, <c>false</c> otherwise.
		/// <see cref="TryResult{T}.Result"/> contains valid object if operation was successful, <see cref="HttpStatusCode.None"/> otherwise.
		/// </returns>
		public static TryResult<String> TryGetDescription(this HttpStatusCode code)
		{
			TryResult<String> result;

			return httpStatusDescriptions.TryGetValue(code, out result) ? result : getDescriptionFailResult;
		}

		#endregion
	}
}