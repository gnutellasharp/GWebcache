using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWebCache.HelperClasses
{
    public class GWebCacheClientConfig
    {
        public string ClientName { get; set; }
        public string Version { get; set; }
        public string UserAgent => $"{ClientName}/{Version}";
        public static GWebCacheClientConfig Default => new GWebCacheClientConfig { ClientName = "CSGN", Version = "0.0.1" };
    }
}
