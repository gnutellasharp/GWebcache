using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GWebCache.Reponses;

namespace GWebCache.ReponseProcessing
{
    internal interface IParseable<T> where T : GWebCacheResponse
    {
        abstract void Parse(HttpResponseMessage? response);
    }
}
