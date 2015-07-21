using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SXN.Web
{
	/// <summary>
	/// Specifies the configuration settings for a <see cref="HttpServerBase"/> class.
	/// </summary>
	[DataContract]
	public class HttpServerSettings : ServerSettings
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="HttpServerSettings"/> class.
		/// </summary>
		/// <param name="name">The name of the server.</param>
		/// <param name="performanceCounters">The dictionary of configuration settings for the performance counters.</param>
		/// <param name="listener">The configuration settings of the http listener.</param>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="performanceCounters"/> is <c>null</c>.</exception>
		public HttpServerSettings(String name, IReadOnlyDictionary<String, PerformanceCounterSettings> performanceCounters, HttpListenerSettings listener)
			: base(name, performanceCounters)
		{
			Listener = listener;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The configuration settings of the http listener.
		/// </summary>
		[DataMember]
		public HttpListenerSettings Listener
		{
			get;
		}

		#endregion
	}
}