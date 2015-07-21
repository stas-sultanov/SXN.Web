using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SXN.Web
{
	/// <summary>
	/// Provides a set of unit tests for <see cref="HttpRequestPattern"/> class.
	/// </summary>
	[TestClass]
	[ExcludeFromCodeCoverage]
	public sealed class HttpRequestPatternTests
	{
		#region Test methods

		[TestMethod]
		[TestCategory("LoadTests")]
		public void ParseUrlPatternTest()
		{
			var testSample = new HttpRequestPattern
				(
				HttpMethod.Get,
				"/postback/{campaign_id}?{transaction_id}=[transactionId]&{redirect_id}=[redirectId]",
				0,
				"Get-Postback",
				(server, context, time) => null,
				null
				);

			Assert.AreEqual(2, testSample.urlSegments.Count);

			Assert.IsFalse(testSample.urlSegments[0].IsVariable);

			Assert.AreEqual("postback", testSample.urlSegments[0].Name);

			Assert.IsTrue(testSample.urlSegments[1].IsVariable);

			Assert.AreEqual("{campaign_id}", testSample.urlSegments[1].Name);
		}

		#endregion
	}
}