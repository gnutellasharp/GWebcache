using GWebCache.Extensions;
namespace GWebCache.Reponses;

public class PongResponse : GWebCacheResponse {
	public string Message { get; set; }

	public override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!responseMessage.ContentAsString().Contains("pong", StringComparison.InvariantCultureIgnoreCase)) {
			return false;
		}

		return base.IsValidResponse(responseMessage);
	}

	public override void Parse(HttpResponseMessage? response) {
		Message = response?.ContentAsString() ?? "";
	}
}
