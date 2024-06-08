using GWebCache.Client;
using GWebCache.Test.Mock_caches;
using Moq;
using Moq.Protected;
using System;

namespace GWebCache.Test.Clients;
[TestClass]
public class GWebCacheClientTests {
	private readonly string baseUrl = "http://test.be";
	private Mock<GWebCacheHttpClient>? httpClient;
	private GWebCacheClient? client;

	[TestMethod]
	[DataRow("sdffff")]
	[DataRow("")]
	[DataRow(null)]
	[ExpectedException(typeof(ArgumentException))]
	public void ConstructorShouldThrowException(string url) {
		GWebCacheClient client = new(url);
	}


	private void Setup() {
		httpClient = new([GWebCacheClientConfig.Default, new Uri(baseUrl)]);
		client = new(httpClient.Object);
	}

	[TestMethod]
	public void TestPingCall() {
		Setup();
		string shouldCall = $"{baseUrl}/?ping=1";

		client!.Ping();
		httpClient!.Protected().Verify("GetAsync", Times.Once(),shouldCall);
	}

	[TestMethod]
	public void TestCheckIfAlive() {
		Setup();
		string shouldCall = $"{baseUrl}/?ping=1";

		client!.CheckIfAlive();
		httpClient!.Protected().Verify("GetAsync", Times.Once(),shouldCall);
	}

	[TestMethod]
	public void TestStatsCall() {
		Setup();
		string shouldCall = $"{baseUrl}/?stats=1";

		client!.GetStats();
		httpClient!.Protected().Verify("GetAsync", Times.Once(), shouldCall);
	}
}
