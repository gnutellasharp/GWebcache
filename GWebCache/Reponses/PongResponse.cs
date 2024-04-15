using GWebCache.Extensions;
namespace GWebCache.Reponses;

public class PongResponse : GWebCacheResponse {
	public string? Message { get; set; }

	public override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;

		return !responseMessage!.ContentAsString().Contains("pong", StringComparison.InvariantCultureIgnoreCase);
	}

	public override void Parse(HttpResponseMessage response) {
		Message = response?.ContentAsString() ?? "";
	}

	public override void ParseV2(HttpResponseMessage response) {
		throw new NotImplementedException();
	}
}
