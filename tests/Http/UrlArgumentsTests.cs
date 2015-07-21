using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SXN.Web
{
	/// <summary>
	/// Provides a set of unit tests for <see cref="UrlArguments"/> class.
	/// </summary>
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class UrlArgumentsTests
	{
		#region Constant and Static Fields

		private static readonly String[] loadTestSamples =
		{
			"/0ZpVzpXKOkSg_cqeEzjdNw==/101?r=http%3A%2F%2Fmsdn.microsoft.com%2Fquery%2Fdev12.query%3FappId%3DDev12IDEF1%26l%3DEN-US%26k%3Dk(System.Net.WebRequest)%3Bk(TargetFrameworkMoniker-.NETFramework%2CVersion%253Dv4.5.1)%3Bk(DevLang-csharp)%26rd%3Dtrue",
			"/0ZpVzpXKOkSg_cqeEzjdNw==/sutehut?r=https%3A%2F%2Fwww.google.com.ua%2Fwebhp%3Fsourceid%3Dchrome-instant%26ion%3D1%26espv%3D2%26es_th%3D1%26ie%3DUTF-8%23newwindow%3D1%26safe%3Doff%26q%3Dsend%2520life%2520sms%2520ua",
			"/ZpVzpXKOkSg_cqeEzjdNw==/09309_33333?r=https%3A%2F%2Fwww.myget.org%2Ffeed%2Fadrout-common%2Fpackage%2FAdRout.Common.Azure",
			"/0ZpVzpXKOkSg_cqeEzjdNw==/989084iiii989?r=https://www.google.com.ua/webhp?sourceid=chrome-instant&ion=1&espv=2&es_th=1&ie=UTF-8#newwindow=1&safe=off&q=encode+url",
			"/0ZpVzpXKOkSg_cqeEzjdNw==/989084iiii989"
		};

		private static readonly IReadOnlyList<Tuple<String, IList<KeyValuePair<String, String>>>> parsQueryTestSamples = new[]
		{
			new Tuple<String, IList<KeyValuePair<String, String>>>
				(
				"o={transaction_id}&sid={source_id}&s={referrer}&red={is_redirect}",
				new[]
				{
					new KeyValuePair<String, String>("o", "{transaction_id}"),
					new KeyValuePair<String, String>("sid", "{source_id}"),
					new KeyValuePair<String, String>("s", "{referrer}"),
					new KeyValuePair<String, String>("red", "{is_redirect}")
				}
				),
			new Tuple<String, IList<KeyValuePair<String, String>>>
				(
				"r={redirect_url}&t={redirect_method}&c={redirect_http_code}&o={transaction_id}&s={referrer}&sid={source_id}",
				new[]
				{
					new KeyValuePair<String, String>("r", "{redirect_url}"),
					new KeyValuePair<String, String>("t", "{redirect_method}"),
					new KeyValuePair<String, String>("c", "{redirect_http_code}"),
					new KeyValuePair<String, String>("o", "{transaction_id}"),
					new KeyValuePair<String, String>("s", "{referrer}"),
					new KeyValuePair<String, String>("sid", "{source_id}")
				}
				)
		};

		private static readonly Tuple<Uri, Int32>[] testSamples =
		{
			Tuple.Create(new Uri("/redirect/AQAAAAAAAAAAAAAAAAAAAQ==/?ref=http://www.adrout.net", UriKind.Relative), 3),
			Tuple.Create(new Uri("/srr/AQAAAAAAAAAAAAAAAAAAAQ==/101?r=http://www.adrout.net", UriKind.Relative), 3),
			Tuple.Create(new Uri("/0ZpVzpXKOkSg_cqeEzjdNw==/101?r=http://www.adrout.net&a=http://www.rateads.net", UriKind.Relative), 2),
			Tuple.Create(new Uri("/suppa", UriKind.Relative), 1),
			Tuple.Create(new Uri("/suppa/duppa?", UriKind.Relative), 2),
			Tuple.Create(new Uri("/suppa/duppa", UriKind.Relative), 2),
			Tuple.Create(new Uri("/suppa?r=http://holly.shit.com", UriKind.Relative), 1),
			Tuple.Create(new Uri("/suppa/duppa?r=http://holly.shit.com&s=true", UriKind.Relative), 2),
			Tuple.Create(new Uri("/0ZpVzpXKOkSg_cqeEzjdNw==/101?r=http%3A%2F%2Fmsdn.microsoft.com%2Fquery%2Fdev12.query%3FappId%3DDev12IDEF1%26l%3DEN-US%26k%3Dk(System.Net.WebRequest)%3Bk(TargetFrameworkMoniker-.NETFramework%2CVersion%253Dv4.5.1)%3Bk(DevLang-csharp)%26rd%3Dtrue", UriKind.Relative), 2)
		};

		#endregion

		#region Test methods

		[TestMethod]
		[TestCategory("LoadTests")]
		public void ParseCompareLoadTest()
		{
			var testResult = LoadTest.ExecuteCompare("Custom", index =>
			{
				var subIndex = index % loadTestSamples.Length;

				var sample = loadTestSamples[subIndex];

				var uriParams = UrlArguments.TryParse(sample).Result;
			}, "Native", index =>
			{
				var subIndex = index % loadTestSamples.Length;

				var sample = loadTestSamples[subIndex];

				var uri = new Uri(sample, UriKind.Relative);
			}, 1000000);

			Trace.TraceInformation(testResult.ToString());

			//Assert.AreEqual(actualResult1, actualResult2);
		}

		[TestMethod]
		[TestCategory("LoadTests")]
		public void ParseLoadTest()
		{
			var testResult = LoadTest.Execute("Custom", index =>
			{
				var subIndex = index % loadTestSamples.Length;

				var sample = loadTestSamples[subIndex];

				var uriParams = UrlArguments.TryParse(sample).Result;
			}, 1000000);

			Trace.TraceInformation(testResult.ToString());

			//Assert.AreEqual(actualResult1, actualResult2);
		}

		[TestMethod]
		[TestCategory("UnitTests")]
		public void ParseUnitTest()
		{
			for (var index = 0; index < testSamples.Length; index++)
			{
				var testSample = testSamples[index];

				var actualResult = UrlArguments.TryParse(testSample.Item1.OriginalString);

				// Check well formed
				var expectedResult = testSample.Item1.IsWellFormedOriginalString();

				Assert.AreEqual(expectedResult, actualResult.Success);

				// Compare segments count
				Assert.AreEqual(testSample.Item2, actualResult.Result.Segments.Count);

				/*
				// Compare segments
				for (var segmentIndex = 0; index < testSample.Item1.Segments.Length; index++)
				{
					var expectedSegment = testSample.Item1.Segments[segmentIndex + 1].TrimStart('/');

					var actualSegment = actualResult.Result.Segments[segmentIndex];

					Assert.AreEqual(expectedSegment, actualSegment);
				}*/
			}
		}

		[TestMethod]
		[TestCategory("UnitTests")]
		public void ParseQuery()
		{
			for (var index = 0; index < parsQueryTestSamples.Count; index++)
			{
				// Get test sample
				var testSample = parsQueryTestSamples[index];

				var tryParseQueryResult = UrlArguments.TryParseQuery(testSample.Item1, 0).Result;

				// Compare arguments count
				Assert.AreEqual(testSample.Item2.Count, tryParseQueryResult.Count);

				// Compare pairs
				for (var pairIndex = 0; pairIndex < testSample.Item2.Count; pairIndex++)
				{
					var expectedPair = testSample.Item2[index];

					var actualPair = tryParseQueryResult.ElementAt(index);

					Assert.AreEqual(expectedPair, actualPair);
				}
			}
		}

		#endregion
	}
}