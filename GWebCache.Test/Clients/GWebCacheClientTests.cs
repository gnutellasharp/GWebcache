using GWebCache.Client;
using Moq;
using Moq.Protected;

namespace GWebCache.Test.Clients;
[TestClass]
public class GWebCacheClientTests {
	private const string BaseUrl = "http://test.be";
	private Mock<GWebCacheHttpClient>? _httpClient;
	private GWebCacheClient? _client;

	[TestMethod]
	[DataRow("ThisIsNotAValidUrl")]
	[DataRow("")]
	[DataRow(null)]
	public void ConstructorShouldThrowException(string url) {
		Assert.ThrowsExactly<ArgumentException>(()=> new GWebCacheClient(url));
	}


	private void Setup() {
		_httpClient = new Mock<GWebCacheHttpClient>([GWebCacheClientConfig.Default, new Uri(BaseUrl)]);
		_client = new GWebCacheClient(_httpClient.Object);
	}

	[TestMethod]
	public void TestPingCall() {
		Setup();
		const string shouldCall = $"{BaseUrl}/?ping=1";

		_client!.Ping();
		_httpClient!.Protected().Verify("GetAsync", Times.Once(),shouldCall);
	}

	[TestMethod]
	public void TestCheckIfAlive() {
		Setup();
		const string shouldCall = $"{BaseUrl}/?ping=1";

		_client!.CheckIfAlive();
		_httpClient!.Protected().Verify("GetAsync", Times.Once(),shouldCall);
	}

	[TestMethod]
	public void TestStatsCall() {
		Setup();
		const string shouldCall = $"{BaseUrl}/?stats=1";

		_client!.GetStats();
		_httpClient!.Protected().Verify("GetAsync", Times.Once(), shouldCall);
	}
}
