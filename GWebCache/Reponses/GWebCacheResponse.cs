using GWebCache.Extensions;
using GWebCache.ReponseProcessing;
namespace GWebCache.Reponses;

public abstract class GWebCacheResponse : IParseable<GWebCacheResponse>, IResponseValidator {
	public virtual bool IsValidResponse(HttpResponseMessage? responseMessage) {
		return responseMessage != null && responseMessage.IsSuccessStatusCode && responseMessage.Content != null;
	}

	public virtual bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		if (IsValidResponse(responseMessage)) 
			return false;

		//V2 permits "empty" responses
		return !string.IsNullOrEmpty(responseMessage!.ContentAsString().Trim());
	}

	public abstract void Parse(HttpResponseMessage response);
	public abstract void ParseV2(HttpResponseMessage response);
}
