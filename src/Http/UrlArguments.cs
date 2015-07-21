using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SXN.Web
{
	/// <summary>
	/// Represents a request arguments sent within the URL.
	/// </summary>
	public struct UrlArguments
	{
		#region Constant and Static Fields

		private static readonly TryResult<Dictionary<String, String>> tryParseQueryFailResult = TryResult<Dictionary<String, String>>.CreateFail();

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="UrlArguments"/> class.
		/// </summary>
		/// <param name="segments">The list of segments within the URL.</param>
		/// <param name="query">The query within the URL as list of key/value pairs.</param>
		private UrlArguments(List<String> segments, Dictionary<String, String> query)
			: this()
		{
			Segments = segments;

			Query = query;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The query within the URL.
		/// </summary>
		public Dictionary<String, String> Query
		{
			get;
		}

		/// <summary>
		/// The collection of the segments within the URL.
		/// </summary>
		public List<String> Segments
		{
			get;
		}

		#endregion

		#region Private methods

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Dictionary<String, String> ParseQuery(String url, Int32 startIndex)
		{
			var result = new Dictionary<String, String>();

			var lastIndex = url.Length;

			// While the end of the url is not reached
			while (startIndex < lastIndex)
			{
				// Get index of the separator
				var separatorIndex = url.IndexOf('=', startIndex);

				if ((separatorIndex == -1) || (separatorIndex == url.Length - 1))
					// The URL is malformed
				{
					return null;
				}

				// Get key
				var key = url.Substring(startIndex, separatorIndex - startIndex);

				// Update current index
				startIndex = separatorIndex + 1;

				// Search for delimiter
				var valueEndIndex = url.IndexOf('&', startIndex);

				if (valueEndIndex == -1)
				{
					// Argument is last
					valueEndIndex = url.Length;
				}

				// Get value
				var value = url.Substring(startIndex, valueEndIndex - startIndex);

				// Add to result
				result[key] = value;

				// Update current index
				startIndex = valueEndIndex + 1;
			}

			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static List<String> ParseSegments(String url, Int32 currentIndex, Int32 endIndex)
		{
			// Initialize result variable
			var result = new List<String>();

			// While the end of the URL is not reached
			while (currentIndex < endIndex)
			{
				currentIndex++;

				// Look for delimiter
				var segmentEndIndex = url.IndexOf('/', currentIndex);

				if (segmentEndIndex != -1 && segmentEndIndex <= endIndex)
				{
					// Calc segment length
					var segmentLength = segmentEndIndex - currentIndex;

					// Cut segment from the URL
					var segment = url.Substring(currentIndex, segmentLength);

					// Add to result
					result.Add(segment);

					// Increment current index
					currentIndex += segmentLength;
				}
				else
				{
					// Calc segment length
					var segmentLength = endIndex - currentIndex;

					// Cut segment from the URL
					var segment = url.Substring(currentIndex, segmentLength);

					// Add to result
					result.Add(segment);

					break;
				}
			}

			return result;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Tries to parse the URL string and extract parameters from it.
		/// </summary>
		/// <returns>
		/// An instance of <see cref="TryResult{T}"/> which encapsulates result of the operation.
		/// <see cref="TryResult{T}.Success"/> contains <c>true</c> if operation was successful, <c>false</c> otherwise.
		/// <see cref="TryResult{T}.Result"/> contains valid object if operation was successful, default value otherwise.
		/// </returns>
		public static TryResult<UrlArguments> TryParse(String url)
		{
			if (url == null)
			{
				return TryResult<UrlArguments>.CreateFail();
			}

			// Get query index
			var queryIndex = url.IndexOf('?', 1);

			// Get the index of the end of the segments
			var segmentEndIndex = queryIndex == -1 ? url.Length : queryIndex;

			// Get segments
			var segments = ParseSegments(url, 0, segmentEndIndex);

			// Check if url is malformed
			if (segments == null)
			{
				return TryResult<UrlArguments>.CreateFail();
			}

			// Check if query should be parsed
			if (queryIndex == -1)
			{
				var tresult = new UrlArguments(segments, new Dictionary<String, String>());

				return TryResult<UrlArguments>.CreateSuccess(tresult);
			}

			// Parse query
			var query = ParseQuery(url, queryIndex + 1);

			// Check if url is malformed
			if (query == null)
			{
				return TryResult<UrlArguments>.CreateFail();
			}

			// Return result
			var result = new UrlArguments(segments, query);

			return TryResult<UrlArguments>.CreateSuccess(result);
		}

		/// <summary>
		/// Tries to parse the query part of the url.
		/// </summary>
		/// <param name="url">The URL to parse.</param>
		/// <param name="startIndex">An start index of the query.</param>
		/// <returns>
		/// An instance of <see cref="TryResult{T}"/> which encapsulates result of the operation.
		/// <see cref="TryResult{T}.Success"/> contains <c>true</c> if operation was successful, <c>false</c> otherwise.
		/// <see cref="TryResult{T}.Result"/> contains valid object if operation was successful, <c>null</c> otherwise.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TryResult<Dictionary<String, String>> TryParseQuery(String url, Int32 startIndex)
		{
			// Check data argument
			if (url == null)
			{
				return tryParseQueryFailResult;
			}

			// Check start index
			if ((startIndex < 0) || (startIndex > url.Length))
			{
				return tryParseQueryFailResult;
			}

			var result = ParseQuery(url, startIndex);

			return result == null ? tryParseQueryFailResult : TryResult<Dictionary<String, String>>.CreateSuccess(result);
		}

		#endregion
	}
}