using GWebCache.Client;
using GWebCache.Extensions;
using GWebCache.Models.Enums;
using GWebCache.ReponseProcessing;
using GWebCache.Reponses;
using GWebCache.Requests;
using System;
using System.Web;

namespace GWebCache;

/// <summary>
/// Concrete implementation of <see cref="IGWebCacheClient"/>
/// </summary>
public class GWebCacheClient : IGWebCacheClient {
	private readonly GWebCacheHttpClient gWebCacheHttpClient;
	private readonly GWebCacheClientConfig gWebCacheClientConfig;
	private readonly Uri _host;

	/// <summary>
	/// Constructor for the GWebCacheClient
	/// </summary>
	/// <param name="host">The url of the webcache in string format</param>
	/// <param name="config">Optional configuration object</param>
	/// <exception cref="ArgumentException">If the url can't be parsed an argument exception is thrown</exception>
	/// <remarks>It's recommended that you use http even if the webcache supports https.</remarks>
	/// <remarks>The constructor will invoke the default configuration if not specified <see cref="GWebCacheClientConfig"/></remarks>
	/// <remarks>The constructor will also check if the webcache is a V2 webcache in case it's not explicitely provided in the configuration</remarks>
	/// <see cref="DetermineIfCacheIsV2"/>
	public GWebCacheClient(string host, GWebCacheClientConfig? config = null) {
		//check that the host is valid
		if (string.IsNullOrWhiteSpace(host) || !Uri.TryCreate(host, new(), out Uri? uri))
			throw new ArgumentException("host was invalid");

		//initalizes fields
		this.gWebCacheClientConfig = config ?? GWebCacheClientConfig.Default;
		gWebCacheHttpClient = new(config: this.gWebCacheClientConfig);
		_host = uri;

		//Determine cache version if the parameter is filled in not applicable
		if (!this.gWebCacheClientConfig.IsV2.HasValue)
			DetermineIfCacheIsV2();
	}

	//constructor used for tests
	internal GWebCacheClient() { }

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

	public Result<HostfileResponse> GetHostfile(GnutellaNetwork? gnutellaNetwork = null) {
		if (WebCacheIsV2()) {
			Result<HostfileResponse> result = new();
			Result<GetResponse> response = Get(gnutellaNetwork);

			if (!response.WasSuccessful || response.ResultObject == null)
				return result.WithException(response.ErrorMessage ?? "Something went wrong getting the correct response");

			result.WasSuccessful = response.WasSuccessful;
			result.ResultObject = new HostfileResponse() { GnutellaNodes = response.ResultObject!.GnutellaNodes };
			return result;
		}
		return GetWithParam<HostfileResponse>("hostfile", "1");
	}

	public Result<UrlFileResponse> GetUrlFile(GnutellaNetwork? network = null) {
		if (WebCacheIsV2()) {
			Result<UrlFileResponse> result = new();
			Result<GetResponse> response = Get(network);

			if (!response.WasSuccessful || response.ResultObject == null)
				return result.WithException(response.ErrorMessage ?? "Something went wrong getting the correct response");

			result.WasSuccessful = response.WasSuccessful;
			result.ResultObject = new UrlFileResponse() { WebCaches = response.ResultObject!.WebCacheNodes };
			return result;
		}

		return GetWithParam<UrlFileResponse>("urlfile", "1");
	}

	public Result<UpdateResponse> Update(UpdateRequest updateRequest) {
		if (!updateRequest.IsValidRequest())
			return new Result<UpdateResponse>().WithException("This request was invalid specify you at least have one cache or node specified and the cache is http.");

		Dictionary<string, object> queryDict = [];
		if(WebCacheIsV2())
			queryDict.Add("update", "1");

		string? networkName = updateRequest.Network.HasValue? Enum.GetName(typeof(GnutellaNetwork), updateRequest.Network.Value) : "";
		if (!string.IsNullOrEmpty(networkName)) 
			queryDict.Add("net", networkName);

		if (updateRequest.GnutellaNode != null) 
			queryDict.Add("ip", HttpUtility.UrlEncode(updateRequest.GnutellaNode.ToString()));
		
		
		if (updateRequest.GWebCacheNode != null) 
			queryDict.Add("url", HttpUtility.UrlEncode(updateRequest.GWebCacheNode.ToString()));

		return PreformGetWithQueryDict<UpdateResponse>(queryDict);
	}

	private Result<T> GetWithParam<T>(string param, string value) where T : GWebCacheResponse, new() {
		Dictionary<string, object> queryDict = new() {
			[param] = value
		};
		return PreformGetWithQueryDict<T>(queryDict);
	}

	private Result<T> PreformGetWithQueryDict<T>(Dictionary<string, object> queryDict) where T : GWebCacheResponse, new() {
		string url = _host.GetUrlWithQuery(queryDict);
		HttpResponseMessage? response = gWebCacheHttpClient.GetAsync(url).Result;
		return new Result<T>().Execute(response);
	}

	public Result<GetResponse> Get(GnutellaNetwork? network) {
		if (!gWebCacheClientConfig.IsV2!.Value) {
			return new Result<GetResponse>().WithException("This method is not supported on a V1 WebCache");
		}

		Dictionary<string, object> queryDict = [];
		
		string? networkName = network.HasValue? Enum.GetName(typeof(GnutellaNetwork), network.Value):"";
		if (!string.IsNullOrEmpty(networkName))
			queryDict.Add("net", networkName);

		queryDict.Add("get", "1");
		return PreformGetWithQueryDict<GetResponse>(queryDict);
	}

	public bool WebCacheIsV2() {
		return gWebCacheClientConfig.IsV2 ?? false;
	}
}
