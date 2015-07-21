using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SXN.Web
{
	/// <summary>
	/// Represents a response to a request being made by HTTP.
	/// </summary>
	public sealed class HttpResponse : IDisposable
	{
		#region Constant and Static Fields

		private const String deflateKeyword = @"deflate";

		private const String gzipKeyword = @"gzip";

		#endregion

		#region Fields

		private readonly HttpListenerResponse response;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="HttpResponse"/> class.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public HttpResponse(HttpListenerResponse response)
		{
			this.response = response;

			this.response.SendChunked = false;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the HTTP status code to be returned to the client.
		/// </summary>
		public HttpStatusCode StatusCode
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (HttpStatusCode) response.StatusCode;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				response.StatusCode = (Int32) value;
			}
		}

		#endregion

		#region Methods of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			response.Close();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Adds the specified <see cref="Cookie"/> to the collection of cookies for this response.
		/// </summary>
		/// <param name="cookie">The <see cref="Cookie"/> to add to the collection to be sent with this response</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddCookie(Cookie cookie)
		{
			response.AppendCookie(cookie);
		}

		/// <summary>
		/// Adds the specified header and value to the HTTP headers for this response.
		/// </summary>
		/// <param name="name">The name of the HTTP header to set.</param>
		/// <param name="value">The value for the name header.</param>
		public void AddHeader(String name, String value)
		{
			response.AddHeader(name, value);
		}

		/// <summary>
		/// Sends a Bad Request response to the client.
		/// </summary>
		/// <param name="keepAlive"></param>
		[SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SendBadRequest(Boolean keepAlive = false)
		{
			// Set status code
			response.StatusCode = (Int32) HttpStatusCode.BadRequest;

			// Set keep alive
			response.KeepAlive = keepAlive;

			// Send
			response.Close();
		}

		/// <summary>
		/// Asynchronously sends <paramref name="content"/> to the client and closes the connection.
		/// </summary>
		/// <param name="content">A <see cref="byte"/> array that contains data to be send to the client.</param>
		/// >
		/// <param name="contentType">MIME type of the returned content.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public async Task SendContentAsync(Byte[] content, String contentType)
		{
			// Set status code
			response.StatusCode = (Int32) HttpStatusCode.OK;

			// Set a MIME type of the returned content
			response.ContentType = contentType;

			// Set content length
			response.ContentLength64 = content.LongLength;

			// Write data into the output stream
			await response.OutputStream.WriteAsync(content, 0, content.Length);

			response.Close();
		}

		/// <summary>
		/// Asynchronously sends <paramref name="content"/> to the client and closes the connection.
		/// </summary>
		/// <param name="content">A <see cref="Stream"/> that contains data to be send to the client.</param>
		/// >
		/// <param name="contentType">MIME type of the returned content.</param>
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public async Task SendContentAsync(Stream content, String contentType)
		{
			// Set status code
			response.StatusCode = (Int32) HttpStatusCode.OK;

			// Set a MIME type of the returned content
			response.ContentType = contentType;

			// Set content length
			response.ContentLength64 = content.Length;

			// Write data into the output stream
			await content.CopyToAsync(response.OutputStream);

			response.Close();
		}

		/// <summary>
		/// Adds data into the response entity.
		/// </summary>
		/// <param name="content">A <see cref="Stream"/> that contains data to be send to the client.</param>
		/// >
		/// <param name="contentType">MIME type of the returned contentBuffer.</param>
		/// <param name="contentCompression">Encoding algorithms supported by the client.</param>
		[SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		[SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public async Task SendContentAsync(Stream content, String contentType, HttpCompression contentCompression)
		{
			// Prefer to use deflate as best speed/ratio algorithm
			if ((contentCompression & HttpCompression.Deflate) == HttpCompression.Deflate)
				// Compress response entity with deflate
			{
				// Add an encoding header to the HTTP headers for this response
				response.AddHeader(HttpHeader.ContentEncoding.TryGetName(), deflateKeyword);

				using (var memoryStream = new MemoryStream((Int32) content.Length))
				{
					// Initialize a new instance of the DeflateStream class
					using (var compressStream = new DeflateStream(memoryStream, CompressionMode.Compress, true))
					{
						// Write compressed bytes to the underlying stream from the specified byte array
						await content.CopyToAsync(compressStream);
					}

					memoryStream.Position = 0;

					await SendContentAsync(memoryStream, contentType);

					return;
				}
			}

			if ((contentCompression & HttpCompression.Gzip) == HttpCompression.Gzip)
				// Compress response entity with gzip
			{
				// Add an encoding header to the HTTP headers for this response
				response.AddHeader(HttpHeader.ContentEncoding.TryGetName(), gzipKeyword);

				using (var memoryStream = new MemoryStream())
				{
					// Initialize a new instance of the GZipStream class
					using (var compressStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
					{
						// Write compressed bytes to the underlying stream from the specified byte array
						content.CopyTo(compressStream);
					}

					memoryStream.Position = 0;

					await SendContentAsync(memoryStream, contentType);

					return;
				}
			}

			// No compression
			await SendContentAsync(content, contentType);
		}

		/// <summary>
		/// Sends a response to the client.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SendOk()
		{
			// Set status code
			response.StatusCode = (Int32) HttpStatusCode.OK;

			// Send response to the client and release the resources held by this HttpListenerResponse instance
			response.Close();
		}

		/// <summary>
		/// Sends the response to redirect the web client to the specified URL.
		/// </summary>
		/// <param name="url">The URL that the client should use to locate the requested resource.</param>
		/// <param name="statusCode">The <see cref="HttpStatusCode"/> to use for redirect. Could be one of the following 301, 302, 303, 307.</param>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SendRedirect(String url, HttpStatusCode statusCode = HttpStatusCode.Found)
		{
			// Set header
			response.Headers[HttpResponseHeader.Location] = url;

			// Set status code
			response.StatusCode = (Int32) statusCode;

			// Send
			response.Close();
		}

		/// <summary>
		/// Adds the specified header and value to the HTTP headers for this response.
		/// </summary>
		/// <param name="header">The <see cref="HttpHeader"/> to add.</param>
		/// <param name="value">The value of the header.</param>
		public Boolean TryAddHeader(HttpHeader header, String value)
		{
			var headerName = header.TryGetName();

			if (!headerName.Success)
			{
				return false;
			}

			response.AddHeader(headerName.Result, value);

			return true;
		}

		#endregion
	}
}