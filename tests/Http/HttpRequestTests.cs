using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SXN.Web
{
	/// <summary>
	/// Provides a set of unit tests for <see cref="HttpRequest"/> class.
	/// </summary>
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class HttpRequestTests
	{
		#region Get Compression

		private const Int32 loadTestIterationCount = 16777216;

		private static readonly Tuple<String, HttpCompression>[] compressionTestSamples =
		{
			Tuple.Create("bzip2,compress,exi,peerdist,sdch", HttpCompression.None), Tuple.Create("bzip2,compress,exi,peerdist,identity,sdch", HttpCompression.Identity), Tuple.Create("bzip2,exi,gzip,peerdist,sdch", HttpCompression.Gzip), Tuple.Create("bzip2,compress,deflate,exi,peerdist,sdch", HttpCompression.Deflate), Tuple.Create("bzip2,compress,deflate,exi,gzip,peerdist,sdch", HttpCompression.Deflate | HttpCompression.Gzip), Tuple.Create("bzip2,compress,deflate,exi,identity,gzip,peerdist,sdch", HttpCompression.Deflate | HttpCompression.Gzip | HttpCompression.Identity)
		};

		[TestMethod]
		[TestCategory("UnitTests")]
		public void GetCompression()
		{
			foreach (var testSample in compressionTestSamples)
			{
				var actualResult = HttpRequest.GetCompression(testSample.Item1, false);

				Assert.AreEqual(testSample.Item2, actualResult);
			}
		}

		[TestMethod]
		[TestCategory("LoadTests")]
		public void GetCompressionLoadTest()
		{
			var testSamplesCount = compressionTestSamples.Length;

			var testResult = LoadTest.Execute("Get compression", index =>
			{
				var subIndex = index % testSamplesCount;

				var inputValue = compressionTestSamples[subIndex].Item1;

				HttpRequest.GetCompression(inputValue, false);
			}, 16777216);

			Trace.TraceInformation(testResult.ToString());
		}

		#endregion

		#region Get Method

		private static readonly Tuple<String, HttpMethod>[] getMethodTestSamples =
		{
			Tuple.Create(@"CONNECT", HttpMethod.Connect), Tuple.Create(@"DELETE", HttpMethod.Delete), Tuple.Create(@"GET", HttpMethod.Get), Tuple.Create(@"HEAD", HttpMethod.Head), Tuple.Create(@"OPTIONS", HttpMethod.Options), Tuple.Create(@"PATCH", HttpMethod.Patch), Tuple.Create(@"POST", HttpMethod.Post), Tuple.Create(@"PUT", HttpMethod.Put), Tuple.Create(@"TRACE", HttpMethod.Trace), Tuple.Create(@"SHI", HttpMethod.None), Tuple.Create(@"SHIT", HttpMethod.None), Tuple.Create(@"SHITT", HttpMethod.None), Tuple.Create(@"BIGTTSHITT", HttpMethod.None)
		};

		[TestMethod]
		[TestCategory("UnitTests")]
		public void GetHttpMethod()
		{
			foreach (var testSample in getMethodTestSamples)
			{
				var actualResult = HttpRequest.GetHttpMethod(testSample.Item1);

				Assert.AreEqual(testSample.Item2, actualResult);
			}
		}

		[TestMethod]
		[TestCategory("LoadTests")]
		public void GetHttpMethodLoadTest()
		{
			var testResult = LoadTest.Execute("Dictionary", index =>
			{
				var subIndex = index % getMethodTestSamples.Length;

				var testSample = getMethodTestSamples[subIndex];

				HttpRequest.GetHttpMethod(testSample.Item1);
			}, loadTestIterationCount);

			Trace.TraceInformation(testResult.ToString());
		}

		#endregion
	}
}