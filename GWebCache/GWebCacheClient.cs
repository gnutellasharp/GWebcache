using GWebCache.Client;
using GWebCache.Extensions;
using GWebCache.Models.Enums;
using GWebCache.ReponseProcessing;
using GWebCache.Reponses;
using GWebCache.Requests;
using System.Web;

namespace GWebCache;

/// <summary>
/// Concrete implementation of <see cref="IGWebCacheClient"/>
/// </summary>
public class GWebCacheClient : IGWebCacheClient {
	private readonly GWebCacheHttpClient gWebCacheHttpClient;

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
		if (config == null)
			config = GWebCacheClientConfig.Default;


		//Determine cache version if the parameter is filled in not applicable
		config.IsV2 = config.IsV2.HasValue ? config.IsV2.Value : CheckIfCacheIsV2();

		gWebCacheHttpClient = new(config,uri);
	}

	//constructor used for tests
	internal GWebCacheClient(GWebCacheHttpClient client) {
		gWebCacheHttpClient = client;
	}

	private bool CheckIfCacheIsV2() {
		Result<PongResponse> pingResult = Ping();
		return pingResult.IsV2Response;
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
			result.ResultObject = new UrlFileResponse() { WebCacheNodes = response.ResultObject!.WebCacheNodes };
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
		
		
		if (updateRequest.WebCacheNode != null) 
			queryDict.Add("url", HttpUtility.UrlEncode(updateRequest.WebCacheNode.ToString()));

		return PreformGetWithQueryDict<UpdateResponse>(queryDict);
	}

	private Result<T> GetWithParam<T>(string param, string value) where T : GWebCacheResponse, new() {
		Dictionary<string, object> queryDict = new() {
			[param] = value
		};
		return PreformGetWithQueryDict<T>(queryDict);
	}

	private Result<T> PreformGetWithQueryDict<T>(Dictionary<string, object> queryDict) where T : GWebCacheResponse, new() {
		if(gWebCacheHttpClient.BaseUri == null)
			throw new InvalidOperationException("The base uri is not set");
		
		string url = gWebCacheHttpClient.BaseUri.GetUrlWithQuery(queryDict);
		HttpResponseMessage? response = gWebCacheHttpClient.GetAsync(url).Result;
		return new Result<T>().Execute(response);
	}

	public Result<GetResponse> Get(GnutellaNetwork? network) {
		if (!WebCacheIsV2()) {
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
		return gWebCacheHttpClient.config.IsV2 ?? false;
	}
}
