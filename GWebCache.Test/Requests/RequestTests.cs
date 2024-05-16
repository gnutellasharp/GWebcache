using GWebCache.Models.Enums;
using GWebCache.ReponseProcessing;
using GWebCache.Reponses;
using GWebCache.Test.Mock_caches;

namespace GWebCache.Test.Requests;


[TestClass]
public class RequestTests {
	public static IEnumerable<object[]> Caches {
		get {
			return new[] {
				[new GhostWhiteCrabCache()],
				[new BazookaCache()],
				[new DKACCache()],
				[new SkullsCache()],
				new object[]{new BeaconCache()},
			};
		}
	}

	[TestMethod]
	[DynamicData(nameof(Caches))]
	public void CheckIfPingIsParsedSuccesfully(IMockCache mockCache) {
		Result<PongResponse> result = new Result<PongResponse>();
		HttpResponseMessage response = new HttpResponseMessage();
		response.Content = new StringContent(mockCache.GetPongRespone());
		result.Execute(response);
		Assert.IsTrue(result.WasSuccessful);
		Assert.IsNotNull(result.ResultObject);
		Assert.IsTrue(string.IsNullOrEmpty(result.ErrorMessage));
		Assert.AreEqual(result.ResultObject.CacheVersion, mockCache.GetVersion());
		Assert.IsTrue(Enumerable.SequenceEqual(result.ResultObject.SupportedNetworks, mockCache.GetSupportedNetworks()));
		Assert.AreEqual(result.IsV2Response, mockCache.IsV2Cache());
	}

	[TestMethod]
	[DynamicData(nameof(Caches))]
	public void CheckIfGetIsParsedSuccesfully(IMockCache mockCache) {
		if (mockCache.IsV2Cache()) {
			Result<GetResponse> result = new Result<GetResponse>();
			HttpResponseMessage response = new HttpResponseMessage();
			response.Content = new StringContent(mockCache.GetGetResponse(GnutellaNetwork.Gnutella2));
			result.Execute(response);
			Assert.IsTrue(result.WasSuccessful);
			Assert.IsNotNull(result.ResultObject);
			Assert.IsTrue(string.IsNullOrEmpty(result.ErrorMessage));
			Assert.AreEqual(result.IsV2Response, mockCache.IsV2Cache());
			Assert.IsTrue(Enumerable.SequenceEqual(mockCache.GetUrls(), result.ResultObject.WebCacheNodes.Select(s => s.ToString()).ToArray()));
			Assert.IsTrue(Enumerable.SequenceEqual(mockCache.GetHosts(), result.ResultObject.GnutellaNodes.Select(s => s.ToString()).ToArray()));
			return;
		}
		Assert.IsTrue(true);
	}

	[TestMethod]
	[DynamicData(nameof(Caches))]
	public void CheckIfHostFileParsedSuccesfully (IMockCache mockCache) {
		if (!mockCache.SupportsV1())
			return;

		Result<HostfileResponse> result = new Result<HostfileResponse>();
		HttpResponseMessage response = new HttpResponseMessage();
		response.Content = new StringContent(mockCache.GetHostfileResponse());
		result.Execute(response);
		Assert.IsTrue(result.WasSuccessful);
		Assert.IsNotNull(result.ResultObject);
		Assert.IsTrue(string.IsNullOrEmpty(result.ErrorMessage));
		Assert.IsFalse(result.IsV2Response);
		Assert.IsTrue(Enumerable.SequenceEqual(mockCache.GetHosts(), result.ResultObject.GnutellaNodes.Select(s => s.ToString()).ToArray()));
	}

	[TestMethod]
	[DynamicData(nameof(Caches))]
	public void CheckIfUrlFileResponseParsedSuccesfully(IMockCache mockCache) {
		if (!mockCache.SupportsV1())
			return;

		Result<UrlFileResponse> result = new();
		HttpResponseMessage response = new() {
			Content = new StringContent(mockCache.GetUrlfileResponse())
		};
		result.Execute(response);
		Assert.IsTrue(result.WasSuccessful);
		Assert.IsNotNull(result.ResultObject);
		Assert.IsTrue(string.IsNullOrEmpty(result.ErrorMessage));
		Assert.IsFalse(result.IsV2Response);
		Assert.IsTrue(Enumerable.SequenceEqual(mockCache.GetUrls(), result.ResultObject.WebCacheNodes.Select(s => s.ToString()).ToArray()));
	}

	[TestMethod]
	[DynamicData(nameof(Caches))]
	public void CheckIfStatFileResponseParsedSuccesfully(IMockCache mockCache) {
		Result<StatFileResponse> result = new();
		HttpResponseMessage response = new() {
			Content = new StringContent(mockCache.GetStatFileResponse())
		};
		result.Execute(response);

		Assert.AreEqual(result.WasSuccessful, mockCache.SupportsStats());

		if(mockCache.SupportsStats()) {
			Assert.IsNotNull(result.ResultObject);
			Assert.IsTrue(string.IsNullOrEmpty(result.ErrorMessage));
			Assert.IsFalse(result.IsV2Response);
			Assert.AreEqual(result.ResultObject.TotalNumberOfRequests, mockCache.GetTotalNumberOfRequests());
			Assert.AreEqual(result.ResultObject.RequestsInLastHour, mockCache.GetNumberOfRequestsInLastHour());
			Assert.AreEqual(result.ResultObject.UpdateRequestsInLastHour, mockCache.GetNumberOfUpdatesInLastHour());
		} else {
			Assert.IsNotNull(result.ErrorMessage);
		}
	}

	[TestMethod]
	[DynamicData(nameof(Caches))]
	public void CheckIfUpdateResponseParsedSuccesfully(IMockCache mockCache) {
		Result<UpdateResponse> result = new();
		HttpResponseMessage response = new() {
			Content = new StringContent(mockCache.GetUpdateReponse(GnutellaNetwork.Gnutella2))
		};
		result.Execute(response);
		Assert.AreEqual(result.WasSuccessful, mockCache.UpdateCallSucceeeded());

		if(!mockCache.UpdateCallSucceeeded()) {
			Assert.IsNotNull(result.ErrorMessage);
			Assert.AreEqual(result.ErrorMessage, mockCache.GetUpdateMessage());
			return;
		}

		Assert.IsNotNull(result.ResultObject);
		Assert.IsTrue(string.IsNullOrEmpty(result.ErrorMessage));
		Assert.AreEqual(result.ResultObject.Message, mockCache.GetUpdateMessage());
	}
}