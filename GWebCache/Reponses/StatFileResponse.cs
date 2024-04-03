using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWebCache.Reponses
{
    public class StatFileResponse : GWebCacheResponse, IParseable<StatFileResponse>
    {
        public int TotalNumberOfRequests { get; set; }
        public int RequestsInLastHour { get; set; }
        public int? UpdateRequestsInLastHour { get; set; }

        public static async Task<StatFileResponse> ParseAsync(HttpResponseMessage? response)
        {
            var result = new StatFileResponse();
            var content = await response?.Content.ReadAsStringAsync() ?? "";
            result.WasSuccessful = GWebCacheResponse.Parse(response).WasSuccessful;

            var lines = content.Split("\n").Select(l=>l.Trim()).Where(l=>!string.IsNullOrEmpty(l)).ToArray();
            result.WasSuccessful = result.WasSuccessful && lines.Length >= 2 && !lines.Any(line=> !int.TryParse(line, out var _));

            if (result.WasSuccessful)
            {
                result.TotalNumberOfRequests = int.Parse(lines[0]);
                result.RequestsInLastHour = int.Parse(lines[1]);
                if (lines.Length > 2)
                {
                    result.UpdateRequestsInLastHour = int.Parse(lines[2]);
                }
            }

            return result;
        }
    }
}
