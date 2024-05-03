using GWebCache.Extensions;
using GWebCache.Models;

namespace GWebCache.Reponses;

/// <summary>
/// A response class containing the other webcaches known
/// </summary>
/// <remarks>For a V2 compliant webcache the <see cref="GetResponse"/> is being used</remarks>
public class UrlFileResponse : GWebCacheResponse {
	public List<GWebCacheNode> WebCaches { get; set; } = new();

	internal override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;

		string content = responseMessage!.ContentAsString();
		if (content.Contains("error", StringComparison.InvariantCultureIgnoreCase))
			return false;

		//validate that all urls are http
		return GetUrlsFromResponse(responseMessage!).All(uri => uri.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase));
	}


	/// <summary>
	/// Parses the HTTP response from the server into a list of other  webcaches
	/// </summary>
	/// <param name="response">The HTTP response from the server</param>
	internal override void Parse(HttpResponseMessage response) {
		foreach(string url in GetUrlsFromResponse(response)) {
			WebCaches.Add(new GWebCacheNode(url));
		}
	}

	/// <summary>
	/// Is never V2 so will return false. <see cref="GetResponse"/>
	/// </summary>
	internal override bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		return false;
	}

	/// <summary>
	/// Not used, internally the <see cref="GetResponse"/> is used for forwards compatibility
	/// </summary>
	/// <param name="response">The HTTP response from the server</param>
	/// <exception cref="NotImplementedException">Will alwats be thrown</exception>
	internal override void ParseV2(HttpResponseMessage response) {
		throw new NotImplementedException();
	}

	/// <summary>
	/// Parses the body of the respons to a string, splits it on new lines and removes empty lines
	/// </summary>
	/// <param name="response">The HTTP response from the server</param>
	/// <returns>An array of string representing urls to other webcaches</returns>
	internal string[] GetUrlsFromResponse(HttpResponseMessage response) {
		return response!.ContentAsString().Split("\n").Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
	}
}
