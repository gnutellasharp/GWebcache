using GWebCache.Reponses;

namespace GWebCache.ReponseProcessing;

public class Result<T> where T : GWebCacheResponse {
	public bool WasSuccessful { get; set; }
	public bool IsV2Response { get; set; }
	public string? ErrorMessage { get; set; }
	public T? ResultObject { get; set; }

	public Result<T> WithException(string exceptionMessage) {
		WasSuccessful = false;
		ErrorMessage = exceptionMessage;
		return this;
	}
	 

	public Result<T> Execute(HttpResponseMessage? responseMessage) {
		ResultObject = (T?)Activator.CreateInstance(typeof(T));
		WasSuccessful = ResultObject?.IsValidResponse(responseMessage) ?? false;
		IsV2Response = WasSuccessful && ResultObject!.IsValidV2Response(responseMessage);

		if (!WasSuccessful) {
			ErrorMessage = responseMessage?.Content?.ReadAsStringAsync()?.Result ?? "";
			return this;
		}

		if (IsV2Response)
			ResultObject?.ParseV2(responseMessage!);
		else
			ResultObject?.Parse(responseMessage!);

		return this;
	}

	/// <summary>
	/// Overidable method for custom response validation
	/// </summary>
	/// <returns></returns>
	public virtual bool IsValidResponse() {
		return true;
	}
}
