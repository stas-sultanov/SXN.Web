using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SXN.Web
{
	public abstract class CampaignRequestHandlerBase : HttpRequestHandlerBase<SupperHttpService>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="HttpRequestHandlerBase{TServer}"/> class.
		/// </summary>
		/// <param name="server">The HTTP server which accepted the request.</param>
		/// <param name="context">An object that encapsulates information about the HTTP request.</param>
		/// <param name="acceptTime">The UTC time when request was accepted by the server.</param>
		protected CampaignRequestHandlerBase(SupperHttpService server, HttpContext context, DateTime acceptTime)
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
			// Try get data from the HTTP request
			var getDataFail = !TryGetData();

			// Check if get data has failed
			if (getDataFail)
			{
				try
				{
					// Send back 400 - bad request
					Context.Response.SendBadRequest();
				}
				catch (HttpListenerException)
				{
				}

				return false;
			}

			// Response
			try
			{
				await SendResponseAsync();
			}
			catch (HttpListenerException)
			{
			}

			return true;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initiates an asynchronous operation to response on the HTTP request.
		/// </summary>
		/// <returns>A <see cref="Task"/> object that represents the asynchronous operation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract Task SendResponseAsync();

		/// <summary>
		/// Tries to get data from the HTTP request.
		/// </summary>
		/// <returns><c>true</c> if operation was successful, <c>false</c> otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected abstract Boolean TryGetData();

		#endregion
	}
}