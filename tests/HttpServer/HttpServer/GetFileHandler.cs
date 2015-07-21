using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SXN.Web
{
	[ExcludeFromCodeCoverage]
	[HttpRequestPattern(HttpMethod.Get, @"/{fileName}", Name = @"Get-File", Order = 0)]
	public sealed class GetFileHandler : HttpRequestHandlerBase<SupperHttpService>
	{
		#region Constructors

		public GetFileHandler(SupperHttpService server, HttpContext context, DateTime acceptTime)
			: base(server, context, acceptTime)
		{
		}

		#endregion

		#region Overrides of HttpRequestHandlerBase<SupperHttpService>

		/// <summary>
		/// Initiates an asynchronous operation to try processes the HTTP request.
		/// </summary>
		/// <returns>
		/// A <see cref="Task{Boolean}"/> object of type <see cref="Boolean"/>> that represents the asynchronous operation.
		/// <see cref="Task{Boolean}.Result"/> equals <c>true</c> if operation has completed successfully, <c>false</c> otherwise.
		/// </returns>
		public override async Task<Boolean> TryProcessAsync()
		{
			var fileName = Context.Request.UrlArguments.Segments[0];

			Byte[] file;

			if (!Server.TryGetFile(fileName, out file))
			{
				return false;
			}

			await Context.Response.SendContentAsync(file, "text");

			return true;
		}

		#endregion
	}
}