using System;

namespace SXN.Web
{
	/// <summary>
	/// Represents the pattern that the HTTP request must match to be handled.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class HttpRequestPatternAttribute : Attribute
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestPatternAttribute"/> class.
		/// </summary>
		/// <param name="method">The HTTP web method.</param>
		/// <param name="urlPattern">The pattern of the url.</param>
		public HttpRequestPatternAttribute(HttpMethod method, String urlPattern)
		{
			// Set method
			Method = method;

			// Set pattern
			UrlPattern = urlPattern;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The maximal length of the content.
		/// </summary>
		public Int32 MaxContentLength
		{
			get;

			set;
		}

		/// <summary>
		/// The HTTP web method.
		/// </summary>
		public HttpMethod Method
		{
			get;
		}

		/// <summary>
		/// The name.
		/// </summary>
		public String Name
		{
			get;

			set;
		}

		/// <summary>
		/// The global order.
		/// </summary>
		public Int32 Order
		{
			get;

			set;
		}

		/// <summary>
		/// The url pattern.
		/// </summary>
		public String UrlPattern
		{
			get;
		}

		#endregion
	}
}