using GWebCache.Extensions;
namespace GWebCache.Reponses;

/// <summary>
/// Represents a pong response from a GWebCache server. This is mainly used to check if the server is alive.
/// In rare cases this also provides suplimental information about the cache.
/// </summary>
public class PongResponse : GWebCacheResponse {

	/// <summary>
	/// This can contain which vendor the cache is running.
	/// </summary>
	/// <remarks>It's only a minority of caches who fill this in</remarks>
	public string? CacheVersion { get; set; }

	/// <summary>
	/// The <see cref="GnutellaNetwork"/> the cache supports
	/// </summary>
	/// <remarks>Again it's a miniority of the caches who actually supply this info. Don't rely on it being there.</remarks>
	public string? SupportedNet { get; set; }

	/// <summary>
	/// A message is valid if it complies with <see cref="GWebCacheResponse.IsValidResponse(HttpResponseMessage?)"/> 
	/// and the response contains pong as a string.
	/// </summary>
	/// <param name="responseMessage">The HTTP response returned from the request</param>
	/// <returns>Boolean indicating if the webresponse can be parsed</returns>
	internal override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;

		return responseMessage!.ContentAsString().Contains("pong", StringComparison.InvariantCultureIgnoreCase);
	}

	/// <summary>
	/// A message is valid if it complies with <see cref="GWebCacheResponse.IsValidV2Response(HttpResponseMessage?)"/>
	/// and the response contains pong as a string, starts with I and has at least 2 fields.
	/// </summary>
	/// <param name="responseMessage"></param>
	/// <returns></returns>
	internal override bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		if (!IsValidResponse(responseMessage))
			return false;

		string[] fields = responseMessage!.SplitContentInFields();
		return fields.Length >= 2 && fields[0].Equals("I", StringComparison.InvariantCultureIgnoreCase) 
			&& fields[1].Equals("pong", StringComparison.InvariantCultureIgnoreCase);
	}

	/// <summary>
	/// Parses the HTTP response from the server into a pong response.
	/// Everything after pong will be marked as the cache version.
	/// </summary>
	/// <param name="response"></param>
	internal override void Parse(HttpResponseMessage response) {
		//Get the version of the cache v1 requests are typically -> "pong" "cache version"
		CacheVersion = response.ContentAsString().Replace("pong", "", StringComparison.InvariantCultureIgnoreCase).Trim();
	}


	internal override void ParseV2(HttpResponseMessage response) {
		string[] fields = response.SplitContentInFields();
		CacheVersion = fields.Length > 2 ? fields[2] : "";
		SupportedNet = fields.Length > 3 ? fields[3] : "";
	}
}
