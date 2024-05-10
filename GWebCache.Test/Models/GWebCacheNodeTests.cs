using GWebCache.Models;

namespace GWebCache.Test.Models;

[TestClass]
public class GWebCacheNodeTests {
	[TestMethod]
	[DataRow("https://test.com")]
	[DataRow("https://www.test.com/bla")]
	[DataRow("http://www.test.com/")]
	[DataRow("http://test.com")]
	public void TestConstructor(string url) {
		Uri uri = new(url);
		GWebCacheNode node = new GWebCacheNode(url);
		Assert.IsNotNull(node);
		Assert.AreEqual(node.Url, uri);

		node = new(uri);
		Assert.IsNotNull(node);
		Assert.AreEqual(node.Url, uri);
	}

	[TestMethod]
	[DataRow("sdffff")]
	[DataRow("")]
	[DataRow(null)]
	[ExpectedException(typeof(ArgumentException))]
	public void TestConstructorShouldThrowException(string url) {
		GWebCacheNode node = new(url);
	}


	[TestMethod]
	[DataRow("http://www.test.com/")]
	public void TestEquals(string url) {
		Uri uri = new(url);
		GWebCacheNode node = new(url);
		GWebCacheNode node2 = new(uri);
		Assert.IsTrue(node.Equals(node2));

	}
	[TestMethod]
	[DataRow("http://www.test.com/")]
	public void TestToString(string url) {
		Uri uri = new(url);
		GWebCacheNode node = new(uri);
		Assert.AreEqual(node.ToString(), uri.ToString());
	}
}