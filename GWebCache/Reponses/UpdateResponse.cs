using GWebCache.Extensions;

namespace GWebCache.Reponses;

public class UpdateResponse : GWebCacheResponse {
	public string? Message { get; set; }

	internal override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;
		string content = responseMessage!.ContentAsString();
		return content.Contains("ok", StringComparison.InvariantCultureIgnoreCase);
	}

	internal override bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		if (!IsValidResponse(responseMessage))
			return false;

		string[] fields = responseMessage!.SplitContentInFields();
		return fields.Length >= 2 && fields[0].Equals("I", StringComparison.InvariantCultureIgnoreCase);
	}

	internal override void Parse(HttpResponseMessage response) {
		Message = response?.ContentAsString() ?? "";
	}

	internal override void ParseV2(HttpResponseMessage response) {
		string[] fields = response!.SplitContentInFields();
		Message = fields.Last().Equals("ok", StringComparison.CurrentCultureIgnoreCase)? "": fields.Last() ;
	}
}
