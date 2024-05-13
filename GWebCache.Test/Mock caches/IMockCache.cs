using GWebCache.Models.Enums;

namespace GWebCache.Test.Mock_caches;
public interface IMockCache  {
	public string GetPongRespone();
	public string GetGetResponse(GnutellaNetwork? net);
	public string GetHostfileResponse();
	public string GetUrlfileResponse();
	public string GetUpdateReponse(GnutellaNetwork? net);
	public string GetStatFileResponse();

	//property for test
	public string GetVersion();
	public string[] GetSupportedNetworks();
	public bool IsV2Cache();
	public string[] GetHosts();
	public string[] GetUrls();
	public bool SupportsV1();
}
