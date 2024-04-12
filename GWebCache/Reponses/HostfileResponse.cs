using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GWebCache.ReponseProcessing;

namespace GWebCache.Reponses
{
    public class HostfileLine
    {
        public IPAddress IPAddress { get; set; }
    }

    public class HostfileResponse: GWebCacheResponse { 
        public List<HostfileLine> HostfileLines { get; set; } = new List<HostfileLine>();

        public override void Parse(HttpResponseMessage? response)
        {
            throw new NotImplementedException();
        }
    }
}
