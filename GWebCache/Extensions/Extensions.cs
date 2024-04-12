using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWebCache.Extensions
{
    public static class Extensions
    {
        public static string ContentAsString(this HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync()?.Result ?? "";
        }

        public static string GetUrlWithQuery(this Uri uri, Dictionary<string,string> queryParams)
        {
            var parameters = QueryHelpers.ParseQuery(uri.Query).ToDictionary(x => x.Key, x => x.Value.First());
            foreach (var param in queryParams)
            {
                if (!parameters.ContainsKey(param.Key))
                {
                    parameters.Add(param.Key, param.Value);
                }
            }

            string baseurl = !string.IsNullOrEmpty(uri.Query) ? uri.AbsoluteUri.Replace(uri.Query, "") : uri.AbsoluteUri;
            return QueryHelpers.AddQueryString(baseurl, parameters);
        }
    }
}
