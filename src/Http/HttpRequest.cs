using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace SXN.Web
{
	/// <summary>
	/// Describes an incoming HTTP request.
	/// </summary>
	public sealed class HttpRequest
	{
		#region Constant and Static Fields

		private static readonly TryResult<String> getContentAsStringFailResult = TryResult<String>.CreateFail();

		#endregion

		#region Fields

		/// <summary>
		/// The types of compression which are supported by the client.
		/// </summary>
		private HttpCompression? acceptCompression;

		/// <summary>
		/// A string representation of the content sent with request.
		/// </summary>
		private TryResult<String>? contentAsStringResult;

		/// <summary>
		/// A type of content com
		/// </summary>
		private HttpCompression? contentCompression;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance of <see cref="HttpRequest"/> class.
		/// </summary>
		internal HttpRequest(HttpListenerRequest request)
		{
			// 0. Check arguments
			if (request == null)
			{
				IsMalformed = true;

				return;
			}

			// 1. Get HTTP method
			Method = GetHttpMethod(request.HttpMethod);

			if (Method == HttpMethod.None)
			{
				IsMalformed = true;

				return;
			}

			// 2. Get URL
			Url = request.RawUrl;

			// 2.1 Get URL parameters
			var urlArgumentsResult = UrlArguments.TryParse(Url);

			if (!urlArgumentsResult.Success)
			{
				IsMalformed = true;

				return;
			}

			UrlArguments = urlArgumentsResult.Result;

			// 3. Get client end point
			RemoteEndPoint = request.RemoteEndPoint;

			// 4. Get headers
			StandardHeaders = new Dictionary<HttpHeader, String>();

			CustomHeaders = new Dictionary<String, String>();

			for (var index = 0; index < request.Headers.Count; index++)
			{
				var headerAsString = request.Headers.GetKey(index);

				var headerParseResult = request.Headers.GetKey(index).TryGetId();

				if (headerParseResult.Success)
				{
					StandardHeaders[headerParseResult.Result] = request.Headers.Get(index);
				}
				else
				{
					CustomHeaders[headerAsString] = request.Headers.Get(index);
				}
			}

			// Get cookies
			Cookies = request.Cookies;

			// Get content
			if (!request.HasEntityBody)
			{
				return;
			}

			// Copy content into the internal buffer
			Content = new Byte[request.ContentLength64];

			try
			{
				request.InputStream.Read(Content, 0, (Int32) request.ContentLength64);
			}
			catch (IOException)
			{
				IsMalformed = true;
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the list of compression types which are accepted by the client.
		/// </summary>
		public HttpCompression AcceptCompression
		{
			get
			{
				if (acceptCompression.HasValue)
				{
					return acceptCompression.Value;
				}

				String headerValue;

				acceptCompression = StandardHeaders.TryGetValue(HttpHeader.AcceptEncoding, out headerValue) ? GetCompression(headerValue, false) : HttpCompression.None;

				return acceptCompression.Value;
			}
		}

		/// <summary>
		/// Gets the content of the request.
		/// </summary>
		[SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
		public Byte[] Content
		{
			get;
		}

		/// <summary>
		/// Gets the type of compression of the content.
		/// </summary>
		public HttpCompression ContentCompression
		{
			get
			{
				if (contentCompression.HasValue)
				{
					return contentCompression.Value;
				}

				String headerValue;

				contentCompression = StandardHeaders.TryGetValue(HttpHeader.ContentEncoding, out headerValue) ? GetCompression(headerValue, true) : HttpCompression.None;

				return contentCompression.Value;
			}
		}

		/// <summary>
		/// Gets the length of the body data included in the request.
		/// </summary>
		public UInt32 ContentLength => Content == null ? 0u : (UInt32) Content.Length;

		/// <summary>
		/// Gets the cookies sent with the request.
		/// </summary>
		public CookieCollection Cookies
		{
			get;

			private set;
		}

		/// <summary>
		/// Gets the collection of the custom HTTP headers sent with the request.
		/// </summary>
		public IDictionary<String, String> CustomHeaders
		{
			get;
		}

		/// <summary>
		/// Gets the DNS name and, if provided, the port number specified by the client.
		/// </summary>
		public String Host => StandardHeaders[HttpHeader.Host];

		/// <summary>
		/// Gets a <see cref="Boolean"/> value which indicates whether the HTTP request is malformed.
		/// </summary>
		public Boolean IsMalformed
		{
			get;
		}

		/// <summary>
		/// Gets the HTTP method specified by the client.
		/// </summary>
		public HttpMethod Method
		{
			get;
		}

		/// <summary>
		/// Gets the client IP address and port number from which the request originated.
		/// </summary>
		public IPEndPoint RemoteEndPoint
		{
			get;
		}

		/// <summary>
		/// Gets the collection of the standard HTTP headers sent with the request.
		/// </summary>
		public IDictionary<HttpHeader, String> StandardHeaders
		{
			get;
		}

		/// <summary>
		/// Gets the relative form of the URL requested by the client.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		public String Url
		{
			get;
		}

		/// <summary>
		/// Gets the arguments of the request sent within the URL.
		/// </summary>
		public UrlArguments UrlArguments
		{
			get;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Parses the string and looking for the <see cref="HttpCompression"/>.
		/// </summary>
		/// <param name="value">A <see cref="string"/> to be parsed.</param>
		/// <param name="singleValue">A <c>Boolean</c> value which specifies whether method should look for single or multiple values of <see cref="HttpCompression"/>.</param>
		/// <returns>Content encodings.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static HttpCompression GetCompression(String value, Boolean singleValue)
		{
			var result = HttpCompression.None;

			if (value == null)
			{
				return result;
			}

			for (var index = 0; index < value.Length;)
			{
				switch (value[index])
				{
						#region deflate

					case 'd':
					{
						if (index + 7 > value.Length)
						{
							goto default;
						}

						var word = (UInt64) (value[index + 1] | value[index + 2] << 8 | value[index + 3] << 16 | value[index + 4] << 24);

						word |= (UInt64) value[index + 5] << 32 | (UInt64) value[index + 6] << 40;

						if (word != 0x6574616C6665)
						{
							goto default;
						}

						if (singleValue)
						{
							return HttpCompression.Deflate;
						}

						result |= HttpCompression.Deflate;

						index += 7;

						continue;
					}

						#endregion

						#region gzip

					case 'g':
					{
						if (index + 4 > value.Length)
						{
							goto default;
						}

						var word = value[index + 1] | value[index + 2] << 8 | value[index + 3] << 16;

						if (word != 0x70697A)
						{
							goto default;
						}

						if (singleValue)
						{
							return HttpCompression.Gzip;
						}

						result |= HttpCompression.Gzip;

						index += 4;

						continue;
					}

						#endregion

						#region identity

					case 'i':
					{
						if (index + 8 > value.Length)
						{
							goto default;
						}

						var word = (UInt64) (value[index + 1] | value[index + 2] << 8 | value[index + 3] << 16 | value[index + 4] << 24);

						word |= (UInt64) value[index + 5] << 32 | (UInt64) value[index + 6] << 40 | (UInt64) value[index + 7] << 48;

						if (word != 0x797469746E6564)
						{
							goto default;
						}

						if (singleValue)
						{
							return HttpCompression.Identity;
						}

						result |= HttpCompression.Identity;

						index += 8;

						continue;
					}

						#endregion

					default:
					{
						index++;

						break;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Gets the <see cref="HttpMethod"/> from the string.
		/// </summary>
		/// <param name="httpMethod">A string representation of the HTTP method.</param>
		/// <returns>The HTTP method.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static HttpMethod GetHttpMethod(String httpMethod)
		{
			if (httpMethod == null)
			{
				return HttpMethod.None;
			}

			switch (httpMethod.Length)
			{
				case 3:
				{
					var longValue = httpMethod[0] << 0 | httpMethod[1] << 8 | httpMethod[2] << 16;

					if (longValue == 0x0000000000544547)
					{
						return HttpMethod.Get;
					}

					if (longValue == 0x0000000000545550)
					{
						return HttpMethod.Put;
					}

					goto default;
				}

				case 4:
				{
					var longValue = httpMethod[0] << 0 | httpMethod[1] << 8 | httpMethod[2] << 16 | httpMethod[3] << 24;

					if (longValue == 0x0000000044414548)
					{
						return HttpMethod.Head;
					}

					if (longValue == 0x0000000054534F50)
					{
						return HttpMethod.Post;
					}

					goto default;
				}

				case 5:
				{
					Int64 longValue = httpMethod[0] | httpMethod[1] << 8 | httpMethod[2] << 16 | httpMethod[3] << 24;

					longValue |= (Int64) httpMethod[4] << 32;

					if (longValue == 0x0000004843544150)
					{
						return HttpMethod.Patch;
					}

					if (longValue == 0x0000004543415254)
					{
						return HttpMethod.Trace;
					}

					goto default;
				}

				case 6:
				{
					Int64 longValue = httpMethod[0] | httpMethod[1] << 8 | httpMethod[2] << 16 | httpMethod[3] << 24;

					longValue |= (Int64) httpMethod[4] << 32 | (Int64) httpMethod[5] << 40;

					if (longValue == 0x00004554454C4544)
					{
						return HttpMethod.Delete;
					}

					goto default;
				}

				case 7:
				{
					Int64 longValue = httpMethod[0] | httpMethod[1] << 8 | httpMethod[2] << 16 | httpMethod[3] << 24;

					longValue |= (Int64) httpMethod[4] << 32 | (Int64) httpMethod[5] << 40 | (Int64) httpMethod[6] << 48;

					if (longValue == 0x005443454E4E4F43)
					{
						return HttpMethod.Connect;
					}

					if (longValue == 0x00534E4F4954504F)
					{
						return HttpMethod.Options;
					}

					goto default;
				}

				default:
				{
					return HttpMethod.None;
				}
			}
		}

		/// <summary>
		/// Gets input stream using encoding provided.
		/// </summary>
		/// <param name="inputStream">A base input stream.</param>
		/// <param name="contentCompression">A contentBuffer encoding.</param>
		/// <returns>The input stream.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Stream GetInputStream(Stream inputStream, HttpCompression contentCompression)
		{
			switch (contentCompression)
			{
				case HttpCompression.Gzip:
				{
					return new GZipStream(inputStream, CompressionMode.Decompress, false);
				}

				case HttpCompression.Deflate:
				{
					return new DeflateStream(inputStream, CompressionMode.Decompress, false);
				}

				default:
				{
					return inputStream;
				}
			}
		}

		/// <summary>
		/// Tries to gets the content of the request as string.
		/// </summary>
		/// <returns>
		/// An instance of <see cref="TryResult{T}"/> which encapsulates result of the operation.
		/// <see cref="TryResult{T}.Success"/> contains <c>true</c> if operation was successful, <c>false</c> otherwise.
		/// <see cref="TryResult{T}.Result"/> contains valid object if operation was successful, <c>null</c> otherwise.
		/// </returns>
		public TryResult<String> TryGetContentAsString()
		{
			if (contentAsStringResult.HasValue)
			{
				return contentAsStringResult.Value;
			}

			using (var memoryStream = new MemoryStream(Content))
			{
				// Get input stream
				var inputStream = GetInputStream(memoryStream, ContentCompression);

				// Initialize a new instance of the StreamReader class for the input stream
				using (var streamReader = new StreamReader(inputStream, Encoding.UTF8))
				{
					try
					{
						// Read all characters from the start to the end of the stream
						var result = streamReader.ReadToEnd();

						contentAsStringResult = TryResult<String>.CreateSuccess(result);
					}
					catch (InvalidOperationException)
					{
						contentAsStringResult = getContentAsStringFailResult;
					}
				}
			}

			return contentAsStringResult.Value;
		}

		#endregion
	}
}