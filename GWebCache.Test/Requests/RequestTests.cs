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
	}

}
