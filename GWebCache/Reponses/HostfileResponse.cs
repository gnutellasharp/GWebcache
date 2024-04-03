using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GWebCache.Reponses
{
    public class HostfileLine
    {
        public IPAddress IPAddress { get; set; }
    }

    public class HostfileResponse: GWebCacheResponse, IParseable<HostfileResponse>
    {
        public string ErrorMessage { get; set; }
        public List<HostfileLine> HostfileLines { get; set; } = new List<HostfileLine>();

        public static async Task<HostfileResponse> ParseAsync(HttpResponseMessage? response)
        {
            var result = new HostfileResponse();
            var content = await response?.Content.ReadAsStringAsync() ?? "";
            result.WasSuccessful = GWebCacheResponse.Parse(response).WasSuccessful;

            result.WasSuccessful = result.WasSuccessful && !string.IsNullOrEmpty(content);

            return result;
        }
    }
}
