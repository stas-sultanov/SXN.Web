using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace SXN.Web
{
	[ExcludeFromCodeCoverage]
	[HttpRequestPattern(HttpMethod.Post, @"/data/{campaignId}/{transactionId}", Name = @"Post-Collected-Data", MaxContentLength = 32768, Order = 1)]
	public sealed class PostSnifferResponseHandler : CampaignRequestHandlerBase
	{
		#region Constant and Static Fields

		public const String RouteRootSegment = @"data";

		#endregion

		#region Fields

		private Object campaign;

		#endregion

		#region Constructors

		public PostSnifferResponseHandler(SupperHttpService server, HttpContext context, DateTime acceptTime)
			: base(server, context, acceptTime)
		{
		}

		#endregion

		#region Overrides of CampaignRequestHandlerBase

		protected override Task SendResponseAsync()
		{
			return Context.Response.SendContentAsync(Encoding.UTF8.GetBytes(@"ProcessSnifferResponse"), "text");
		}

		protected override Boolean TryGetData()
		{
			// 0. Get request arguments
			var requestArguments = Context.Request.UrlArguments;

			// Try get campaign information
			if (!Server.TryGetCampaign(requestArguments.Segments[1], out campaign))
			{
				goto BADREQUEST;
			}

			// 3. Try get requestId
			var requestId = Value128Converter.TryFromBase64String(requestArguments.Segments[2], 0, Base64Encoding.Lex);

			if (!requestId.Success)
			{
				goto BADREQUEST;
			}

			// 4. Try get content as string
			/*var content = Request.ContentAsString;

			if (content == null)
			{
				goto BADREQUEST;
			}*/

			// 6. Try get web client id

			return true;

			BADREQUEST:

			return false;
		}

		#endregion
	}
}