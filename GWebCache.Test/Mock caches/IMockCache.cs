namespace GWebCache.Test.Mock_caches;
internal interface IMockCache  {
	public string GetPongRespone();
	public string GetGetResponse();
	public string GetHostfileResponse();
	public string GetUrlfileResponse();
	public string GetUpdateReponse();
	public string GetStatFileResponse();
}
