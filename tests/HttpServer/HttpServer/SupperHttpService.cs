using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.ServiceModel;
using System.Text;

namespace SXN.Web
{
	/// <summary>
	/// Provides a Web Client Data sniffing service via HTTP.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public sealed class SupperHttpService : HttpServerBase
	{
		#region Fields

		private readonly Dictionary<String, Object> campaigns;

		private readonly Dictionary<String, Byte[]> files;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="SupperHttpService"/> class.
		/// </summary>
		public SupperHttpService(EventHandler<DiagnosticsEventArgs> eventHandler, HttpServerSettings settings)
			: base(eventHandler, settings)
		{
			files = new Dictionary<String, Byte[]>
			{
				{
					"favicon.ico", Encoding.UTF8.GetBytes(@"ProcessFileRequest")
				},
				{
					"movie.swf", Encoding.UTF8.GetBytes(@"ProcessFileRequest")
				}
			};

			campaigns = new Dictionary<String, Object>
			{
				{
					"AAAAQAAQABAAAAAAAAAAAA", new Object()
				}
			};
			/*
			RegisterRoute(GetFile.Route, (server, httpContext) => new GetFile(server, httpContext));

			RegisterRoute(GetRedirectRequest.Route, (server, httpContext) => new GetRedirectRequest(server, httpContext));

			RegisterRoute(PostSnifferResponseHandler.Route, (server, httpContext) => new PostSnifferResponseHandler(server, httpContext));
			*/
			State = EntityState.Inactive;
		}

		#endregion

		#region Methods

		internal Boolean TryGetCampaign(String campaignId, out Object campaign)
		{
			return campaigns.TryGetValue(campaignId, out campaign);
		}

		internal Boolean TryGetFile(String fileName, out Byte[] file)
		{
			return files.TryGetValue(fileName, out file);
		}

		#endregion
	}
}