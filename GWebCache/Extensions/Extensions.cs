using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace GWebCache.Extensions{
	/// <summary>
	/// Extension methods mainly to avoid code reuse
	/// </summary>
	static class Extensions {
		/// <summary>
		/// Gets the Content from an HTTP Response as a string and splits it up with | as a separator
		/// </summary>
		/// <see cref="ContentAsString(HttpResponseMessage)"/>
		internal static string[] SplitContentInFields(this HttpResponseMessage response) {
			string content = ContentAsString(response);
			return content.Split("|").Select(field=>field.Trim()).ToArray();
		}

		/// <summary>
		/// Gets the content from a response and reads it as a string
		/// </summary>
		/// <returns>The content as a string or an empty string if content is null</returns>
		internal static string ContentAsString(this HttpResponseMessage response) {
			return response.Content?.ReadAsStringAsync().Result ?? "";
		}

		/// <summary>
		/// Converts an uri to a string representation and adds query parameters to that string representation
		/// </summary>
		/// <param name="uri">The base uri</param>
		/// <param name="queryParams">A dictionary containing the query parameters as key value pairs</param>
		/// <returns>A url with the query parameters appended</returns>
		/// <remarks>If the query parameter is already part of the uri it will not add it a second time.</remarks>
		internal static string GetUrlWithQuery(this Uri uri, Dictionary<string, string> queryParams) {
			foreach (KeyValuePair<string,string> kv in queryParams) {
				uri.AddQueryParameterToUriIfNotExists(kv.Key, kv.Value);
			}
			
			return uri.ToString();
		}
		

		internal static Uri AddQueryParameterToUriIfNotExists(this Uri uri, string key, string value) {
			UriBuilder builder = new UriBuilder(uri);
			
			Dictionary<string,string> query = builder.Query.Split('&')
				.Select(x => x.Split('='))
				.ToDictionary(x => x.First(), x => x.Last());

			query.TryAdd(key, value);
			builder.Query = string.Join("&", query.Select(kvp => $"{kvp.Key}={kvp.Value}"));
			return builder.Uri;
		}
	}
}