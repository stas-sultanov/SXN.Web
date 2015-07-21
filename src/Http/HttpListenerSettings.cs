using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.Serialization;

namespace SXN.Web
{
	/// <summary>
	/// Specifies the configuration settings for the <see cref="HttpListener"/> class.
	/// </summary>
	[DataContract]
	public sealed class HttpListenerSettings
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="HttpListenerSettings"/> class.
		/// </summary>
		/// <param name="authenticationSchemes">The scheme used to authenticate clients.</param>
		/// <param name="ignoreWriteExceptions">A <see cref="Boolean"/> value that specifies whether application receives exceptions that occur when a <see cref="HttpListener"/> sends the response to the client.</param>
		/// <param name="prefixes">The Uniform Resource Identifier (URI) prefixes handled by a <see cref="HttpListener"/> object.</param>
		/// <param name="realm">The realm, or resource partition, associated with a <see cref="HttpListener"/> object.</param>
		public HttpListenerSettings(AuthenticationSchemes authenticationSchemes, Boolean ignoreWriteExceptions, IList<String> prefixes, String realm)
		{
			// Check arguments
			if (prefixes == null)
			{
				throw new ArgumentNullException(nameof(prefixes));
			}

			AuthenticationSchemes = authenticationSchemes;

			IgnoreWriteExceptions = ignoreWriteExceptions;

			Prefixes = prefixes;

			Realm = realm;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The scheme used to authenticate clients.
		/// </summary>
		[DataMember]
		[DefaultValue(AuthenticationSchemes.Anonymous)]
		public AuthenticationSchemes AuthenticationSchemes
		{
			get;
		}

		/// <summary>
		/// A <see cref="Boolean"/> value that specifies whether application receives exceptions that occur when a <see cref="HttpListener"/> sends the response to the client.
		/// </summary>
		[DataMember]
		public Boolean IgnoreWriteExceptions
		{
			get;
		}

		/// <summary>
		/// The Uniform Resource Identifier (URI) prefixes handled by a <see cref="HttpListener"/> object.
		/// </summary>
		[DataMember]
		public IList<String> Prefixes
		{
			get;
		}

		/// <summary>
		/// The realm, or resource partition, associated with a <see cref="HttpListener"/> object.
		/// </summary>
		[DataMember]
		public String Realm
		{
			get;
		}

		#endregion
	}
}