using GWebCache.Extensions;
using GWebCache.Models;

namespace GWebCache.Reponses;

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

	internal override void Parse(HttpResponseMessage response) {
		foreach(string url in GetUrlsFromResponse(response)) {
			WebCaches.Add(new GWebCacheNode(url));
		}
	}

	internal override void ParseV2(HttpResponseMessage response) {
		throw new NotImplementedException();
	}

	internal string[] GetUrlsFromResponse(HttpResponseMessage response) {
		return response!.ContentAsString().Split("\n").Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
	}
}
