using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GWebCache.ReponseProcessing;

namespace GWebCache.Reponses
{
    public class GnutellaNode
    {
        public IPAddress IPAddress { get; set; }
        public int port { get; set; }
    }

    public class HostfileResponse: GWebCacheResponse { 
        public List<GnutellaNode> HostfileLines { get; set; } = new List<GnutellaNode>();

        public override bool IsValidResponse(HttpResponseMessage? responseMessage)
        {
            var response = base.IsValidResponse(responseMessage);
            var content = responseMessage?.Content?.ReadAsStringAsync().Result;
            return response && !string.IsNullOrWhiteSpace(content) && !content.Contains("error", StringComparison.InvariantCultureIgnoreCase);
        }
        public override void Parse(HttpResponseMessage? response)
        {
            var content = response?.Content?.ReadAsStringAsync().Result;
            var lines = content?.Split("\n").Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
            foreach(string line in lines) {
                string[] parts = line.Split(":");
                if (parts.Length == 2)
                {
                    HostfileLines.Add(new GnutellaNode
                    {
                        IPAddress = IPAddress.Parse(parts[0]),
                        port = int.Parse(parts[1])
                    });
                }
            }
        }
    }
}
