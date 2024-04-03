using GWebCache.Reponses;

namespace GWebCache
{
    public interface IGWebCacheClient
    {
       bool CheckIfAlive();
       StatFileResponse GetStats();
        HostfileResponse GetHostfile();
    }
}
