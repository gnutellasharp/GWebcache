using GWebCache.Extensions;
using GWebCache.ReponseProcessing;
namespace GWebCache.Reponses;

/// <summary>
/// Base class for all GWebCache responses.
/// </summary>
public abstract class GWebCacheResponse{

	/// <summary>
	/// All messages are only valid if they had a success status code and their content is not null (can be empty)
	/// </summary>
	/// <param name="responseMessage">The HTTP response of the server</param>
	/// <returns>A boolean indicating if the HTTP response is a valid GWebcache response</returns>
	internal virtual bool IsValidResponse(HttpResponseMessage? responseMessage) {
		return responseMessage != null && responseMessage.IsSuccessStatusCode && responseMessage.Content != null;
	}

	/// <summary>
	/// All messages are valid if they comply with <see cref="IsValidResponse(HttpResponseMessage?)"/> and their body is not an empty string
	/// </summary>
	/// <param name="responseMessage">The HTTP response of the server</param>
	/// <returns>A boolean indicating if the HTTP response follows the V2 implementation of the protocol</returns>
	internal virtual bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		if (!IsValidResponse(responseMessage)) 
			return false;

		//V2 doesn't permit "empty" responses
		return !string.IsNullOrEmpty(responseMessage!.ContentAsString().Trim());
	}

	internal abstract void Parse(HttpResponseMessage response);
	internal abstract void ParseV2(HttpResponseMessage response);
}
