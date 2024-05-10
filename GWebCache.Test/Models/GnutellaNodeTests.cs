using GWebCache.Models;
using System.Net;

namespace GWebCache.Test.Models;

[TestClass]
public class GnutellaNodeTests {
	[TestMethod]
	[DataRow("127.0.0.1",1212)]
	public void TestConstructor(string ip, int port) {
		IPAddress ipParsed = IPAddress.Parse(ip);
		GnutellaNode node = new GnutellaNode(ip, port);

		Assert.AreEqual(node.IPAddress, ipParsed);
		Assert.AreEqual(node.Port, port);

		node = new(ipParsed, port);
		Assert.AreEqual(node.IPAddress, ipParsed);
		Assert.AreEqual(node.Port, port);
	}

	[TestMethod]
	[DataRow("", 1212)]
	[DataRow("127.0.0.1",-1)]
	[DataRow(null, null)]
	[ExpectedException(typeof(ArgumentException))]
	public void TestConstructorShouldThrowException(string ip, int port) {
		GnutellaNode node = new GnutellaNode(ip, port);
	}

	[TestMethod]
	[DataRow("127.0.0.1", 1212, "127.0.0.1:1212")]
	public void TestToString(string ip, int port, string expectedOutput) {
		IPAddress ipParsed = IPAddress.Parse(ip);

		GnutellaNode node = new GnutellaNode(ip, port);
		Assert.AreEqual(node.ToString(), expectedOutput);


		node = new GnutellaNode(ipParsed, port);
		Assert.AreEqual(node.ToString(), expectedOutput);
	}


	[TestMethod]
	[DataRow("127.0.0.1", 1212)]
	public void TestEquals(string ip, int port) {
		IPAddress ipParsed = IPAddress.Parse(ip);

		GnutellaNode node = new GnutellaNode(ip, port);
		GnutellaNode node2 = new GnutellaNode(ipParsed, port);
		Assert.IsTrue(node.Equals(node2));
	}
}
