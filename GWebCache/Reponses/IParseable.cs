using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWebCache.Reponses
{
    internal  interface IParseable<T> where T : GWebCacheResponse
    {
         abstract static Task<T> ParseAsync(HttpResponseMessage? response);
    }
}
