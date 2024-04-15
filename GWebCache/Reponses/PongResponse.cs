using GWebCache.Extensions;
namespace GWebCache.Reponses;

public class PongResponse : GWebCacheResponse {
	public string? CacheVersion { get; set; }

	public override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;

		return responseMessage!.ContentAsString().Contains("pong", StringComparison.InvariantCultureIgnoreCase);
	}

	public override bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		if (!IsValidResponse(responseMessage))
			return false;

		string[] fields = responseMessage!.SplitContentInFields();
		return fields.Count() >= 2 && fields[0].Equals("I", StringComparison.InvariantCultureIgnoreCase) 
			&& fields[1].Equals("pong", StringComparison.InvariantCultureIgnoreCase);
	}

	public override void Parse(HttpResponseMessage response) {
		//Get the version of the cache v1 requests are typically -> "pong" "cache version"
		CacheVersion = response.ContentAsString().Replace("pong", "", StringComparison.InvariantCultureIgnoreCase).Trim();
	}


	public override void ParseV2(HttpResponseMessage response) {
		string[] fields = response.SplitContentInFields();
		if(fields.Length >= 3)
			CacheVersion = fields[2];
	}
}
