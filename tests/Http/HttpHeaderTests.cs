using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SXN.Web
{
	/// <summary>
	/// Provides a set of unit tests for <see cref="HttpHeader"/> enumeration.
	/// </summary>
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class HttpHeaderTests
	{
		#region Test methods

		[TestMethod]
		[TestCategory("UnitTests")]
		public void UniqueIdTest()
		{
			var values = Enum.GetValues(typeof(HttpHeader)).Cast<Int32>();

			Assert.IsFalse(values.HasDuplicates());
		}

		[TestMethod]
		[TestCategory("UnitTests")]
		public void ConvertTest()
		{
			var values = Enum.GetValues(typeof(HttpHeader)).Cast<HttpHeader>();

			foreach (var expectedResult in values)
			{
				var intermediateResult = expectedResult.TryGetName().Result;

				var actualResult = intermediateResult.TryGetId().Result;

				Assert.AreEqual(expectedResult, actualResult);
			}
		}

		#endregion
	}
}