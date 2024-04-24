using GWebCache.Extensions;
using GWebCache.ReponseProcessing;
namespace GWebCache.Reponses;

/// <summary>
/// Base class for all GWebCache responses.
/// </summary>
public abstract class GWebCacheResponse{

	internal virtual bool IsValidResponse(HttpResponseMessage? responseMessage) {
		return responseMessage != null && responseMessage.IsSuccessStatusCode && responseMessage.Content != null;
	}

	internal virtual bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		if (!IsValidResponse(responseMessage)) 
			return false;

		//V2 permits "empty" responses
		return !string.IsNullOrEmpty(responseMessage!.ContentAsString().Trim());
	}

	internal abstract void Parse(HttpResponseMessage response);
	internal abstract void ParseV2(HttpResponseMessage response);
}
