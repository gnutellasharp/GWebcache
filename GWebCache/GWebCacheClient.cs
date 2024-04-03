using GWebCache.HelperClasses;
using GWebCache.Reponses;
using Microsoft.AspNetCore.WebUtilities;

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
            var message = SendPongMessage().Result;
            return message.WasSuccessful;
        }


        public  StatFileResponse GetStats()
        {
            string url = AddToQueryString(_host, "statfile", "1");
            var request =  gWebCacheHttpClient.GetAsync(url).Result;
            return StatFileResponse.ParseAsync(request).Result;
        }

        private async Task<PongResponse> SendPongMessage()
        {
            string url = AddToQueryString(_host, "ping", "1");
            var request = await gWebCacheHttpClient.GetAsync(url);
            return await PongResponse.ParseAsync(request);
        }

        private string AddToQueryString(Uri uri, string key, string value)
        {
            var parameters = QueryHelpers.ParseQuery(uri.Query);
            parameters.Add(key, value);
            string baseurl = !string.IsNullOrEmpty(uri.Query)?uri.AbsoluteUri.Replace(uri.Query, ""): uri.AbsoluteUri;
            return QueryHelpers.AddQueryString(baseurl, parameters.ToDictionary(x => x.Key, x => x.Value.First()));
        }

        public HostfileResponse GetHostfile()
        {
            string url = AddToQueryString(_host, "hostfile", "1");
            //var request = await gWebCacheHttpClient.GetAsync(url);
            //return await PongResponse.ParseAsync(request);
            return null;
        }
    }
}
