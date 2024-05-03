using GWebCache.Reponses;

namespace GWebCache.ReponseProcessing;
/// <summary>
/// A wrapper object to gracefully handle errors.
/// </summary>
/// <typeparam name="T">The response type that you want to wrap</typeparam>
/// <example><c>Result<PongResponse></c></example>
public class Result<T> where T : GWebCacheResponse {

	/// <summary>
	/// The request completed succefully.
	/// </summary>
	/// <remarks>Succesfull completion is defined by every response individually.</remarks>
	/// <see cref="GWebCacheResponse.IsValidResponse(HttpResponseMessage?)"/>
	public bool WasSuccessful { get; set; }

	/// <summary>
	/// The response is in a V2 format.
	/// </summary>
	/// <remarks> mainly used internally for parsing the http respons into the response object</remarks>
	/// <see cref="GWebCacheResponse.IsValidV2Response(HttpResponseMessage?)"/>
	public bool IsV2Response { get; set; }

	/// <summary>
	/// Possible error message returned from the webcache.
	/// </summary>
	public string? ErrorMessage { get; set; }

	/// <summary>
	/// Parsed resulting object will be null in case of error.
	/// </summary>
	public T? ResultObject { get; set; }

	/// <summary>
	/// Initializes a Result object with an error message
	/// </summary>
	internal Result<T> WithException(string exceptionMessage) {
		WasSuccessful = false;
		ErrorMessage = exceptionMessage;
		return this;
	}


	/// <summary>
	/// Converts the http response into a result object.
	/// </summary>
	/// <remarks>
	///		Does this by first validating the response is valid and if it's a V2 message.
	///		If not succesfull then put the body in the error message.
	///		If succesfull use the relevant parse method.
	///		The http Response is requested by the <see cref="Client.GWebCacheHttpClient"/>
	/// </remarks>
	/// <param name="responseMessage">The http response from the webcache</param>
	/// <returns>Result object</returns>
	internal Result<T> Execute(HttpResponseMessage? responseMessage) {
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
}