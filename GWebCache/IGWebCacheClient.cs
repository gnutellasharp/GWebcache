using GWebCache.Models.Enums;
using GWebCache.ReponseProcessing;
using GWebCache.Reponses;
using GWebCache.Requests;

namespace GWebCache;

public interface IGWebCacheClient {
	bool CheckIfAlive();
	bool WebCacheIsV2();
	Result<PongResponse> Ping();
	Result<StatFileResponse> GetStats();
	Result<HostfileResponse> GetHostfile(GnutellaNetwork network = GnutellaNetwork.Gnutella2);
	Result<UrlFileResponse> GetUrlFile(GnutellaNetwork gnutellaNetwork = GnutellaNetwork.Gnutella2);
	Result<UpdateResponse> Update(UpdateRequest updateRequest);
	Result<GetResponse> Get(GnutellaNetwork network);
}