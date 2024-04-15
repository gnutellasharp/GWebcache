namespace GWebCache.ReponseProcessing;

internal interface IResponseValidator {
	bool IsValidResponse(HttpResponseMessage responseMessage);
}
