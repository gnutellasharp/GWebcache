using GWebCache.Extensions;
using GWebCache.Models;

namespace GWebCache.Reponses;

public class UrlFileResponse : GWebCacheResponse {
	public List<GWebCacheNode> WebCaches { get; } = new();

	public override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;

		string content = responseMessage!.ContentAsString();
		if (content.Contains("error", StringComparison.InvariantCultureIgnoreCase))
			return false;

		//validate that all urls are http
		return GetUrlsFromResponse(responseMessage!).All(uri => uri.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase));
	}

	public override void Parse(HttpResponseMessage response) {
		foreach(string url in GetUrlsFromResponse(response)) {
			WebCaches.Add(new GWebCacheNode(url));
		}
	}

	public override void ParseV2(HttpResponseMessage response) {
		throw new NotImplementedException();
	}

	private string[] GetUrlsFromResponse(HttpResponseMessage response) {
		return response!.ContentAsString().Split("\n").Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
	}
}
