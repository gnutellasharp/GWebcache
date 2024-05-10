using GWebCache.Client;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Moq;
using Moq.Protected;
using System;
using System.Net;

namespace GWebCache.Test.Clients;
[TestClass]
public class GWebCacheHttpClientTests {
	Mock<HttpMessageHandler> mockHttpMessageHandler = new();
	private readonly GWebCacheClientConfig config = new() { 
		ClientName = "TestClient",
		Version = "test"
	};
	private GWebCacheHttpClient client;


	public void SetupGenericCalls() {
		mockHttpMessageHandler.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

		client = new GWebCacheHttpClient(config, new HttpClient(mockHttpMessageHandler.Object));
	}

	[TestMethod]
	public void TestThatHttpClientIsCalled() {
		SetupGenericCalls();
		client.GetAsync("http://test.com").Wait();
		mockHttpMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
	}

	[TestMethod]
	public void FailTest() {
		Assert.Fail();
	}

	[TestMethod]
	public void TestThatThereIsQueryParameters() {
		SetupGenericCalls();
		client.GetAsync("http://test.com").Wait();
		HttpRequestMessage message = mockHttpMessageHandler.Invocations.Last().Arguments[0] as HttpRequestMessage;
		Assert.IsFalse(string.IsNullOrEmpty(message?.RequestUri?.Query));
	}

	[TestMethod]
	public void TestThatClientParameterIsAdded() {
		SetupGenericCalls();
		client.GetAsync("http://test.com").Wait();
		HttpRequestMessage message = mockHttpMessageHandler.Invocations.Last().Arguments[0] as HttpRequestMessage;
		Dictionary<string,StringValues> queryParams = QueryHelpers.ParseQuery(message.RequestUri.Query);
		Assert.IsTrue(queryParams.ContainsKey("client"));	
		Assert.AreEqual(config.ClientName, queryParams["client"].ToString());
	}

	[TestMethod]
	public void TestThatVersionParameterIsAdded() {
		SetupGenericCalls();
		client.GetAsync("http://test.com").Wait();
		HttpRequestMessage message = mockHttpMessageHandler.Invocations.Last().Arguments[0] as HttpRequestMessage;
		Dictionary<string, StringValues> queryParams = QueryHelpers.ParseQuery(message.RequestUri.Query);
		Assert.IsTrue(queryParams.ContainsKey("version"));
		Assert.AreEqual(config.Version, queryParams["version"].ToString());
	}

	[TestMethod]
	public void TestThatUserAgentIsSet() {
		SetupGenericCalls();
		client.GetAsync("http://test.com").Wait();
		HttpRequestMessage message = mockHttpMessageHandler.Invocations.Last().Arguments[0] as HttpRequestMessage;
		Assert.AreEqual(config.UserAgent, message.Headers.UserAgent.ToString());
	}

	[TestMethod]
	[DataRow("")]
	[DataRow("sdfsdfdsf")]
	[DataRow(null)]
	public void CallWithBadUrlThrowsException(string url) {
		SetupGenericCalls();
		Assert.ThrowsException<AggregateException>(() => client.GetAsync(url).Wait());
	}
}
