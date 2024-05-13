using GWebCache.Models.Enums;

namespace GWebCache.Test.Mock_caches;
internal interface IMockCache  {
	public string GetPongRespone();
	public string GetGetResponse(GnutellaNetwork? net);
	public string GetHostfileResponse();
	public string GetUrlfileResponse();
	public string GetUpdateReponse();
	public string GetStatFileResponse();
}
