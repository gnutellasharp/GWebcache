using GWebCache.Client;
using GWebCache.Extensions;
using GWebCache.Models.Enums;
using GWebCache.ReponseProcessing;
using GWebCache.Reponses;
using GWebCache.Requests;

namespace GWebCache;

public class GWebCacheClient : IGWebCacheClient {
	private readonly GWebCacheHttpClient gWebCacheHttpClient;
	private readonly GWebCacheClientConfig gWebCacheClientConfig;
	private readonly Uri _host;

	public GWebCacheClient(string host, GWebCacheClientConfig? config = null) {
		if (!string.IsNullOrWhiteSpace(host) && Uri.TryCreate(host, new UriCreationOptions(), out Uri? uri)) {
			this.gWebCacheClientConfig = config ?? GWebCacheClientConfig.Default;
			gWebCacheHttpClient = new(config: this.gWebCacheClientConfig);
			_host = uri;

			//Determine cache version if the parameter is filled in not applicable
			if (this.gWebCacheClientConfig.IsV2 == null) { 
				DetermineIfCacheIsV2();
			}
		} else {
			throw new ArgumentException("Invalid host");
		}
	}

	private void DetermineIfCacheIsV2() {
		Result<PongResponse> pingResult = Ping();
		gWebCacheClientConfig.IsV2 = pingResult.IsV2Response;
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

	public Result<GetResponse> Get(GnutellaNetwork network) {
		if (!gWebCacheClientConfig.IsV2!.Value) {
			return new Result<GetResponse>().WithException("This method is not supported on a V1 WebCache");
		}

		Dictionary<string, string> queryDict = [];

		string? networkName = Enum.GetName(typeof(GnutellaNetwork), network);
		if (!string.IsNullOrEmpty(networkName))
			queryDict.Add("net", networkName);

		queryDict.Add("get", "1");
		return PreformGetWithQueryDict<GetResponse>(queryDict);
	}

	public bool WebCacheIsV2() {
		return gWebCacheClientConfig.IsV2 ?? false;
	}
}
