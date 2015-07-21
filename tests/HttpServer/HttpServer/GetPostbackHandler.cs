using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace SXN.Web
{
	[ExcludeFromCodeCoverage]
	[HttpRequestPattern(HttpMethod.Get, @"/postback/{campaign_id}?{transaction_id}=[transactionId]&{redirect_id}=[redirectId]", Name = @"Get-Postback", Order = 5)]
	public sealed class GetPostbackHandler : CampaignRequestHandlerBase
	{
		#region Fields

		private Object campaign;

		#endregion

		#region Constructors

		public GetPostbackHandler(SupperHttpService server, HttpContext context, DateTime acceptTime)
			: base(server, context, acceptTime)
		{
		}

		#endregion

		#region Overrides of CampaignRequestHandlerBase

		protected override Task SendResponseAsync()
		{
			return Context.Response.SendContentAsync(Encoding.UTF8.GetBytes(@"GetPostbackHandler"), "text");
		}

		protected override Boolean TryGetData()
		{
			Trace.TraceInformation("Accept time: {0}", AcceptTime);

			// Get request arguments
			var requestArguments = Context.Request.UrlArguments;

			// Try get campaign information
			if (!Server.TryGetCampaign(requestArguments.Segments[1], out campaign))
			{
				goto BADREQUEST;
			}

			return true;

			BADREQUEST:

			return false;
		}

		#endregion
	}
}