using GWebCache.Extensions;

namespace GWebCache.Reponses;

public class UpdateResponse : GWebCacheResponse {
	public string? Message { get; set; }

	override public bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;

		return responseMessage!.ContentAsString().Contains("ok", StringComparison.InvariantCultureIgnoreCase);
	}

	public override void Parse(HttpResponseMessage response) {
		Message = response?.ContentAsString() ?? "";
	}

	public override void ParseV2(HttpResponseMessage response) {
		throw new NotImplementedException();
	}
}
