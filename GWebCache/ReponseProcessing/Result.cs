using GWebCache.Reponses;

namespace GWebCache.ReponseProcessing;

public class Result<T> where T : GWebCacheResponse {
	public bool WasSuccessful { get; set; }
	public string ErrorMessage { get; set; }
	public T? ResultObject { get; set; }

	public Result<T> Execute(HttpResponseMessage? responseMessage) {
		ResultObject = (T?)Activator.CreateInstance(typeof(T));
		WasSuccessful = ResultObject?.IsValidResponse(responseMessage) ?? false;

		if (!WasSuccessful && responseMessage.Content != null) {
			ErrorMessage = responseMessage.Content.ReadAsStringAsync()?.Result ?? "";
			return this;
		}

		ResultObject?.Parse(responseMessage);
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
