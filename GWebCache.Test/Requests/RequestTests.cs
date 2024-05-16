using GWebCache.Models;
using GWebCache.Requests;

namespace GWebCache.Test.Requests;
[TestClass]
public class RequestTests {
	[TestMethod]
	public void NoNodesAndCacheIsNotValid() {
		UpdateRequest request = new();
		Assert.IsFalse(request.IsValidRequest());
	}

	[TestMethod]
	public void HttpsWebCacheIsNotValid() {
		GWebCacheNode gWebCacheNode = new GWebCacheNode("https://test.be");
		UpdateRequest request = new() { WebCacheNode = gWebCacheNode};
		Assert.IsFalse(request.IsValidRequest());
	}


	[TestMethod]
	public void ValidityCheck() {
		GnutellaNode node = new GnutellaNode("127.0.0.1", 1212);
		GWebCacheNode gWebCacheNode = new GWebCacheNode("http://test.be");

		UpdateRequest request = new() { GnutellaNode = node};
		Assert.IsTrue(request.IsValidRequest());

		request = new() { WebCacheNode = gWebCacheNode };
		Assert.IsTrue(request.IsValidRequest());

		request = new() { GnutellaNode = node, WebCacheNode = gWebCacheNode };
		Assert.IsTrue(request.IsValidRequest());
	}

	
}
