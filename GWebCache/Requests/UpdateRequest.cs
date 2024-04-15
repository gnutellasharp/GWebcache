using GWebCache.Models;

namespace GWebCache.Requests;

public class UpdateRequest : GWebCacheRequest{
	public GnutellaNode? GnutellaNode { get; set; }
	public GWebCacheNode? GWebCacheNode { get; set; }

	public override bool IsValidRequest() {
		return GnutellaNode != null || GWebCacheNode != null;
	}
}
