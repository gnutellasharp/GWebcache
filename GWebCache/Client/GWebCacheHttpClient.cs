using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace GWebCache.Client;

/// <summary>
/// A wrapper around <see cref="HttpClient"/> that adds default parameters to the request.
/// </summary>
class GWebCacheHttpClient {
	public readonly HttpClient _client;
	private readonly GWebCacheClientConfig config;

   /// <summary>
   /// Initializes the HttpClient and sets the <see cref="GWebCacheClientConfig.UserAgent"/> as the default User-Agent.
   /// </summary>
	internal GWebCacheHttpClient(GWebCacheClientConfig config) {
		this.config = config;
		_client = new HttpClient();
		_client.DefaultRequestHeaders.Add("User-Agent", config.UserAgent);
	}

	/// <summary>
	/// Converts the url to a <see cref="Uri"/> and calls <see cref="GetAsync(Uri)"/>
	/// </summary>
	/// <param name="url">url to call in string format</param>
	internal async Task<HttpResponseMessage?> GetAsync(string url) {
		Uri uri = new(url);
		return await GetAsync(uri);
	}

	/// <summary>
	/// Preforms a get call with the given <paramref name="uri"/> and adds the default parameters if they don't exist.
	/// </summary>
	/// <returns>The HttpResponse resulting from the get call.</returns>
	/// <see cref="AddDefaultParamsIfNotExist(Uri)"/>
	internal async Task<HttpResponseMessage?> GetAsync(Uri uri) {
		Uri url = AddDefaultParamsIfNotExist(uri);
		HttpResponseMessage response = await _client.GetAsync(url);
		return response;
	}

	/// <summary>
	/// Adds <see cref="GWebCacheClientConfig.ClientName"/> and <see cref="GWebCacheClientConfig.Version"/> to the query string of the url if they don't exist.
	/// </summary>
	/// <returns>A new uri with both query parameters added</returns>
	private Uri AddDefaultParamsIfNotExist(Uri uri) {
		Dictionary<string, StringValues> parameters = QueryHelpers.ParseQuery(uri.Query);

		if (!parameters.ContainsKey("client")) {
			parameters.Add("client", config.ClientName);
		}

		if (!parameters.ContainsKey("version")) {
			parameters.Add("version", config.Version);
		}
		string baseUrl = !string.IsNullOrEmpty(uri.Query) ? uri.AbsoluteUri.Replace(uri.Query, "") : uri.AbsoluteUri;
		string url = QueryHelpers.AddQueryString(baseUrl, parameters.ToDictionary(x => x.Key, x => x.Value.First()));
		return new Uri(url);
	}
}
