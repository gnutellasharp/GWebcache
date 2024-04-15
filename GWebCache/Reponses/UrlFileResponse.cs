using GWebCache.Extensions;
using GWebCache.Models;

namespace GWebCache.Reponses;

public class UrlFileResponse : GWebCacheResponse {
	public List<GWebCacheNode> WebCaches { get; } = new();

	public override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;

		return !responseMessage!.ContentAsString().Contains("error", StringComparison.InvariantCultureIgnoreCase);
	}

	public override void Parse(HttpResponseMessage response) {
		string[] urls = response.ContentAsString().Split("\n")
			.Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
		foreach(string url in urls) {
			WebCaches.Add(new GWebCacheNode(url));
		}
	}
}
