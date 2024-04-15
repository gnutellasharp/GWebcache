using System.Web;

namespace GWebCache.Models;
public class GWebCacheNode {
	public Uri? Url { get; set; }

	public GWebCacheNode(string url) {
		Uri.TryCreate(url, UriKind.Absolute, out Uri? uri);
		this.Url = uri;
	}

	public GWebCacheNode(Uri uri) {
		this.Url = uri;
	}

	public override string ToString() {
		return HttpUtility.UrlEncode(Url?.ToString() ?? "");
	}

	public override bool Equals(object? obj) {
		if (obj != null && obj is GWebCacheNode node)
			return node.Url == Url;

		return false;
	}

	public override int GetHashCode() {
		throw new NotImplementedException();
	}
}
