using System;

namespace SXN.Web
{
	/// <summary>
	/// Specifies the HTTP contentBuffer compression.
	/// </summary>
	[Flags]
	public enum HttpCompression
	{
		/// <summary>
		/// None
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// No transformation is used. This is the default value for contentBuffer coding.
		/// </summary>
		Identity = 0x0001,

		/// <summary>
		/// Free and open source lossless Data compression algorithm.
		/// </summary>
		Bzip2 = 0x0002,

		/// <summary>
		/// UNIX "compress" program method
		/// </summary>
		Compress = 0x0004,

		/// <summary>
		/// Despite its name the zlib compression (RFC 1950) should be used (in combination with the deflate compression (RFC 1951)) as described in the RFC 2616. The implementation in the real world however seems to vary between the zlib compression and the (raw) deflate compression. Due to this confusion, gzip has positioned itself as the more reliable default method (March 2011).
		/// </summary>
		Deflate = 0x0008,

		/// <summary>
		/// W3C Efficient XML Interchange
		/// </summary>
		Exi = 0x0010,

		/// <summary>
		/// GNU zip format (described in RFC 1952). This method is the most broadly supported as of March 2011.
		/// </summary>
		Gzip = 0x0020,

		/// <summary>
		/// Network Transfer Format for Java Archives.
		/// </summary>
		Pack200Gzip = 0x0040,

		/// <summary>
		/// Microsoft Peer Content Caching and Retrieval.
		/// </summary>
		Peerdist = 0x0080,

		/// <summary>
		/// Google Shared Dictionary Compression for HTTP.
		/// </summary>
		Sdch = 0x0100
	}
}