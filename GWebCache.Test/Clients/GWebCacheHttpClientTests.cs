using GWebCache.Client;
using Moq;
using Moq.Protected;
using System.Net;

namespace GWebCache.Test.Clients;
[TestClass]
public class GWebCacheHttpClientTests {
	private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new();
	private readonly GWebCacheHttpClient _client;
	private readonly GWebCacheClientConfig _config = new() { 
		ClientName = "TestClient",
		Version = "test"
	};

	public GWebCacheHttpClientTests() {
		_mockHttpMessageHandler.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });
		
		_client = new GWebCacheHttpClient(_config, new HttpClient(_mockHttpMessageHandler.Object));
	}

	[TestMethod]
	public void TestThatHttpClientIsCalled() {
		_client.GetAsync("http://test.com").RunSynchronously();
		_mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
	}

	[TestMethod]
	public void TestThatThereIsQueryParameters() {
		_client.GetAsync("http://test.com").RunSynchronously();
		HttpRequestMessage? message = _mockHttpMessageHandler.Invocations.Last().Arguments[0] as HttpRequestMessage;
		Assert.IsFalse(string.IsNullOrEmpty(message?.RequestUri?.Query));
	}

	[TestMethod]
	public void TestThatClientParameterIsAdded() {
		_client.GetAsync("http://test.com").RunSynchronously();
		HttpRequestMessage? message = _mockHttpMessageHandler.Invocations[^1].Arguments[0] as HttpRequestMessage;
		
		Dictionary<string,string> queryParams = string.IsNullOrEmpty(message?.RequestUri?.Query)?
				new Dictionary<string, string>() : ParseQuery(message.RequestUri.Query);		
		
		Assert.IsNotNull(message?.RequestUri);
		Assert.IsTrue(queryParams.ContainsKey("client"));	
		Assert.AreEqual(_config.ClientName, queryParams["client"]);
	}

	[TestMethod]
	public void TestThatVersionParameterIsAdded() {
		_client.GetAsync("http://test.com").RunSynchronously();
		HttpRequestMessage? message = _mockHttpMessageHandler.Invocations[^1].Arguments[0] as HttpRequestMessage;
		
		Dictionary<string,string> queryParams = string.IsNullOrEmpty(message?.RequestUri?.Query)?
			new Dictionary<string, string>() : ParseQuery(message.RequestUri.Query);		
		
		Assert.IsTrue(queryParams.ContainsKey("version"));
		Assert.AreEqual(_config.Version, queryParams["version"]);
	}

	[TestMethod]
	public void TestThatUserAgentIsSet() {
		_client.GetAsync("http://test.com").RunSynchronously();
		HttpRequestMessage? message = _mockHttpMessageHandler.Invocations[^1].Arguments[0] as HttpRequestMessage;
		Assert.AreEqual(_config.UserAgent, message?.Headers.UserAgent.ToString());
	}

	[TestMethod]
	[DataRow("")]
	[DataRow("ThisIsNotAValidUrl")]
	[DataRow(null)]
	public void CallWithBadUrlThrowsException(string url) {
		Assert.ThrowsExactly<AggregateException>(() => _client.GetAsync(url).RunSynchronously());
	}
	
	private static Dictionary<string,string> ParseQuery(string query) {
		return query.TrimStart('?')
			.Split('&', StringSplitOptions.RemoveEmptyEntries)
			.Select(part => part.Split('='))
			.ToDictionary(parts => parts[0], parts => parts.Length > 1 ? parts[1] : string.Empty);
	}
}
