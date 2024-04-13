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
			if(Uri.TryCreate(url, UriKind.Absolute, out Uri? uri)) {
				WebCaches.Add(new GWebCacheNode {
					Url = uri
				});
			}
		}
	}
}
