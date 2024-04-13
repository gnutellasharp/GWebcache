using GWebCache.ReponseProcessing;
using GWebCache.Reponses;

namespace GWebCache;

public interface IGWebCacheClient {
	bool CheckIfAlive();
	Result<PongResponse> Ping();
	Result<StatFileResponse> GetStats();
	Result<HostfileResponse> GetHostfile();
}
