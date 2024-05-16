using GWebCache.Models;
using GWebCache.Models.Enums;

namespace GWebCache.Requests;

/// <summary>
/// Request model to update the webcache with a new gnutella node or a WebCache Url.
/// </summary>
/// <see cref="GnutellaNode"/>
/// <seealso cref="GWebCacheNode"/>
public class UpdateRequest : GWebCacheRequest{

	public GnutellaNode? GnutellaNode { get; set; }
	public GWebCacheNode? WebCacheNode { get; set; }

	/// <value>Indicating which <see cref="GnutellaNetwork"/> the Webcache or Gnutella node belongs to.</value>
	/// <remarks>Usually only necessary in case you're providing an update to a V2 cache.</remarks>
	public GnutellaNetwork? Network { get; set; }

	/// <summary>
	/// Request is valid if either the GnutellaNode or GWebCacheNode is not null.
	/// If the GWebCacheNode is not null the Url must start with http.
	/// </summary>
	internal override bool IsValidRequest() {
		bool result = WebCacheNode != null || GnutellaNode != null;

		if (WebCacheNode != null) {
			result  = result && WebCacheNode.Url != null && WebCacheNode.Url.Scheme.Equals(Uri.UriSchemeHttp.ToLower());
		}

		return result;
	}
}
