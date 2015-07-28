using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace SXN.Web
{
	/// <summary>
	/// Provides a set of unit tests for <see cref="HttpServerBase"/> class.
	/// </summary>
	[TestClass]
	[ExcludeFromCodeCoverage]
	public sealed class HttpServerBaseTests
	{
		#region Constant and Static Fields

		// 0.5 Get main HttpListener prefix from cscfg file
		private const String mainPrefix = @"http://localhost:22222/";

		private static readonly CancellationToken cancellationToken = CancellationToken.None;

		private static readonly HttpServerSettings settings;

		private static readonly Tuple<String, String, HttpStatusCode, String>[] testSamples =
		{
			Tuple.Create("GET", @"favicon.ico", HttpStatusCode.OK, @"ProcessFileRequest"),
			Tuple.Create("GET", @"movie.swf", HttpStatusCode.OK, @"ProcessFileRequest"),
			Tuple.Create("GET", @"sushimishi.swf", HttpStatusCode.BadRequest, @""),
			Tuple.Create("GET", @"postback/AAAAQAAQABAAAAAAAAAAAA?o=93849839&dcid=http://www.adrout.net", HttpStatusCode.OK, @"GetPostbackHandler"),
			Tuple.Create("GET", @"redirect/AAAAQAAQABAAAAAAAAAAAA?o=93849839&r=http://www.adrout.net", HttpStatusCode.OK, @"GetRedirectHandler"),
			Tuple.Create("GET", @"redirect/AAAAQAAQABAAAAAAAAA?r=http://www.adrout.net", HttpStatusCode.BadRequest, @""),
			Tuple.Create("POST", @"data/AAAAQAAQABAAAAAAAAAAAA/0ZpVzpXKOkSg_cqeEzjdNw", HttpStatusCode.OK, @"GetRedirectHandler"),
			Tuple.Create("POST", @"data/0ZpVzpXKOkSg_cqeEzjdNw/ee0ZpVzpXKOkSg_cqeEzjd", HttpStatusCode.BadRequest, @"")
		};

		#endregion

		#region Constructor

		static HttpServerBaseTests()
		{
			// 0.0 Get configuration as string
			var settingsAsString = File.ReadAllText(@"HttpServerSettings.json");

			// 0.1 Get service pointer manager configuration
			settings = JsonConvert.DeserializeObject<HttpServerSettings>(settingsAsString);

			// 0.6 Add prefix to configuration
			settings.Listener.Prefixes.Add(mainPrefix);

			// Install counters
			settings.PerformanceCounters.Values.Install();
		}

		#endregion

		#region Test methods

		[TestMethod]
		[TestCategory("Manual")]
		public async Task MakeRequestAsync()
		{
			var service = new SupperHttpService(EventHandler, settings);

			await service.ActivateAsync(cancellationToken);

#pragma warning disable 4014

			Task.Run(() => service.RunAsync(cancellationToken), cancellationToken);

#pragma warning restore 4014

			foreach (var item in testSamples)
			{
				var webRequest = WebRequest.Create(mainPrefix + item.Item2);

				webRequest.Credentials = CredentialCache.DefaultCredentials;

				webRequest.Method = item.Item1;

				webRequest.ContentLength = 0;

				try
				{
					using (var response = (HttpWebResponse) await webRequest.GetResponseAsync())
					{
						Assert.AreEqual(item.Item3, (HttpStatusCode) (Int32) response.StatusCode);

						if (response.StatusCode == System.Net.HttpStatusCode.OK)
						{
							var s = await (new StreamReader(response.GetResponseStream()).ReadToEndAsync());

							Assert.AreEqual(item.Item4, s);
						}
					}
				}
				catch (WebException)
				{
					if (item.Item3 != HttpStatusCode.BadRequest)
					{
						throw;
					}
				}
				catch (Exception e)
				{
					Trace.TraceError(e.Message);
				}
			}

			await service.DeactivateAsync(cancellationToken);
		}

		[TestMethod]
		[TestCategory("Manual")]
		public async Task ExternalTestAsync()
		{
			var service = new SupperHttpService(EventHandler, settings);

			await service.ActivateAsync(cancellationToken);

			var runTask = Task.Run(() => service.RunAsync(cancellationToken), cancellationToken);

			await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);

			await service.DeactivateAsync(cancellationToken);

			await runTask;
		}

		#endregion

		#region Private methods

		private static void EventHandler(Object sender, DiagnosticsEventArgs diagnosticsEventArgs)
		{
			Trace.TraceInformation("{0}:{1}:{2}", diagnosticsEventArgs.Source, diagnosticsEventArgs.Level, diagnosticsEventArgs.Message);
		}

		#endregion
	}
}