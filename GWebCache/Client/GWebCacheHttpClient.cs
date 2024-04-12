using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWebCache.Client
{

    internal class GWebCacheHttpClient
    {
        public readonly HttpClient _client;
        private readonly GWebCacheClientConfig config;

        public GWebCacheHttpClient(GWebCacheClientConfig config)
        {
            this.config = config;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", config.UserAgent);
        }

        public async Task<HttpResponseMessage?> GetAsync(string url)
        {

            Uri uri = new Uri(url);
            return await GetAsync(uri);
        }

        public async Task<HttpResponseMessage?> GetAsync(Uri uri)
        {
            var url = AddDefaultParamsIfNotExist(uri);
            var response = await _client.GetAsync(url);
            return response;
        }

        private Uri AddDefaultParamsIfNotExist(Uri uri)
        {
            var parameters = QueryHelpers.ParseQuery(uri.Query);

            if (!parameters.ContainsKey("client"))
            {
                parameters.Add("client", config.ClientName);
            }

            if (!parameters.ContainsKey("version"))
            {
                parameters.Add("version", config.Version);
            }
            var baseUrl = !string.IsNullOrEmpty(uri.Query)? uri.AbsoluteUri.Replace(uri.Query, "") : uri.AbsoluteUri;
            string url = QueryHelpers.AddQueryString(baseUrl, parameters.ToDictionary(x => x.Key, x => x.Value.First()));
            return new Uri(url);
        }
    }

}
