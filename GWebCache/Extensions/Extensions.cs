using Microsoft.AspNetCore.WebUtilities;

namespace GWebCache.Extensions;

/// <summary>
/// Extension methods for various classes.
/// </summary>
static class Extensions {
	internal static string[] SplitContentInFields(this HttpResponseMessage response) {
		string content = ContentAsString(response);
		return [.. content.Split("|")];
	}

	internal static string ContentAsString(this HttpResponseMessage response) {
		return response.Content?.ReadAsStringAsync()?.Result ?? "";
	}

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
