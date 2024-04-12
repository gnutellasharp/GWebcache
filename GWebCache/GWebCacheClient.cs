using GWebCache.Client;
using GWebCache.Extensions;
using GWebCache.ReponseProcessing;
using GWebCache.Reponses;

namespace GWebCache
{
    public class GWebCacheClient : IGWebCacheClient
    {
        private GWebCacheHttpClient gWebCacheHttpClient;
        private Uri _host;

        public GWebCacheClient(string host, GWebCacheClientConfig? config = null) {
            if (!string.IsNullOrWhiteSpace(host) && Uri.TryCreate(host, new UriCreationOptions(), out var uri)){
                gWebCacheHttpClient = new(config: config ?? GWebCacheClientConfig.Default);
                _host = uri;
            }else{
                throw new ArgumentException("Invalid host");
            }
        }

        public bool CheckIfAlive()
        {
            var pingResponse = Ping();
            return pingResponse.WasSuccessful;
        }

        public Result<PongResponse> Ping()
        {
            var queryDict = new Dictionary<string, string> { { "ping", "1" } };
            string url = _host.GetUrlWithQuery(queryDict);
            var response = gWebCacheHttpClient.GetAsync(url).Result;
            return new Result<PongResponse>().Execute(response);
        }

        Result<StatFileResponse> IGWebCacheClient.GetStats()
        {
            var queryDict = new Dictionary<string, string> { { "statfile", "1" } };
            string url = _host.GetUrlWithQuery(queryDict);
            var response = gWebCacheHttpClient.GetAsync(url).Result;
            return new Result<StatFileResponse>().Execute(response);
        }

        Result<HostfileResponse> IGWebCacheClient.GetHostfile()
        {
            throw new NotImplementedException();
        }
    }
}
