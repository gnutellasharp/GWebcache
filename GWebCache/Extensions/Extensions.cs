using Microsoft.AspNetCore.WebUtilities;

namespace GWebCache.Extensions;

/// <summary>
/// Extension methods mainly to avoid code reuse
/// </summary>
static class Extensions {
	/// <summary>
	/// Gets the Content from an Http Response as a string and splits it up with | as a seperator
	/// </summary>
	/// <see cref="ContentAsString(HttpResponseMessage)"/>
	internal static string[] SplitContentInFields(this HttpResponseMessage response) {
		string content = ContentAsString(response);
		return [.. content.Split("|")];
	}

	/// <summary>
	/// Gets the content from a response and reads it as a string
	/// </summary>
	/// <returns>The content as a string or an empty string if content is null</returns>
	internal static string ContentAsString(this HttpResponseMessage response) {
		return response.Content?.ReadAsStringAsync()?.Result ?? "";
	}

	/// <summary>
	/// Converts an uri to a string representation and adds query parameters to that string representation
	/// </summary>
	/// <param name="uri">The base uri</param>
	/// <param name="queryParams">A dictionary containing the query parameters as key value pairs</param>
	/// <returns>A url with the query parameters appended</returns>
	/// <remarks>If the queryparameter is already part of the uri it will not add it a second time.</remarks>
	internal static string GetUrlWithQuery(this Uri uri, Dictionary<string, string> queryParams) {
		Dictionary<string, string?> parameters = QueryHelpers.ParseQuery(uri.Query).ToDictionary(x => x.Key, x => x.Value.First());
		foreach (KeyValuePair<string, string> param in queryParams) {
			if (!parameters.ContainsKey(param.Key)) {
				parameters.Add(param.Key, param.Value);
			}
		}

		string baseurl = !string.IsNullOrEmpty(uri.Query) ? uri.AbsoluteUri.Replace(uri.Query, "") : uri.AbsoluteUri;
		return QueryHelpers.AddQueryString(baseurl, parameters);
	}
}
