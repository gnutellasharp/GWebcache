namespace GWebCache.ReponseProcessing;

internal interface IResponseValidator {
	bool IsValidResponse(HttpResponseMessage? responseMessage);
	bool IsValidV2Response(HttpResponseMessage? responseMessage);
}
