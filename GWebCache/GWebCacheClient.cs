using GWebCache.Client;
using GWebCache.Extensions;
using GWebCache.ReponseProcessing;
using GWebCache.Reponses;
using GWebCache.Requests;

namespace GWebCache;

public class GWebCacheClient : IGWebCacheClient {
	private readonly GWebCacheHttpClient gWebCacheHttpClient;
	private readonly Uri _host;

	public GWebCacheClient(string host, GWebCacheClientConfig? config = null) {
		if (!string.IsNullOrWhiteSpace(host) && Uri.TryCreate(host, new UriCreationOptions(), out Uri? uri)) {
			gWebCacheHttpClient = new(config: config ?? GWebCacheClientConfig.Default);
			_host = uri;
		} else {
			throw new ArgumentException("Invalid host");
		}
	}

	public bool CheckIfAlive() {
		Result<PongResponse> pingResponse = Ping();
		return pingResponse.WasSuccessful;
	}

	public Result<PongResponse> Ping() {
		return GetWithParam<PongResponse>("ping", "1");
	}

	public Result<StatFileResponse> GetStats() {
		return GetWithParam<StatFileResponse>("stats", "1");
	}

	public Result<HostfileResponse> GetHostfile() {
		return GetWithParam<HostfileResponse>("hostfile", "1");
	}

	public Result<UrlFileResponse> GetUrlFile() {
		return GetWithParam<UrlFileResponse>("urlfile", "1");
	}

	public Result<UpdateResponse> Update(UpdateRequest updateRequest) {
		if (!updateRequest.IsValidRequest())
			return new Result<UpdateResponse>().WithException("This request was invalid specify you at least have one cache or node specified and the cache is http.");

		Dictionary<string, string> queryDict = [];

		if (updateRequest.GnutellaNode != null) {
			queryDict.Add("ip", updateRequest.GnutellaNode.ToString());
		} 
		
		if (updateRequest.GWebCacheNode != null) {
			queryDict.Add("url", updateRequest.GWebCacheNode.ToString());
		}

		return PreformGetWithQueryDict<UpdateResponse>(queryDict);
	}

	private Result<T> GetWithParam<T>(string param, string value) where T : GWebCacheResponse, new() {
		Dictionary<string, string> queryDict = new() {
			[param] = value
		};
		return PreformGetWithQueryDict<T>(queryDict);
	}

	private Result<T> PreformGetWithQueryDict<T>(Dictionary<string, string> queryDict) where T : GWebCacheResponse, new() {
		string url = _host.GetUrlWithQuery(queryDict);
		HttpResponseMessage? response = gWebCacheHttpClient.GetAsync(url).Result;
		return new Result<T>().Execute(response);
	}

}
