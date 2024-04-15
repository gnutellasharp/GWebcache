using GWebCache.Models;

namespace GWebCache.Requests;

public class UpdateRequest : GWebCacheRequest{
	public GnutellaNode? GnutellaNode { get; set; }
	public GWebCacheNode? GWebCacheNode { get; set; }

	public override bool IsValidRequest() {
		if (GWebCacheNode != null) {
			return GWebCacheNode.Url != null && GWebCacheNode.Url.Scheme.Equals(Uri.UriSchemeHttp.ToLower());
		}

		return GnutellaNode != null;
	}
}
