using GWebCache.ReponseProcessing;

namespace GWebCache.Requests;

public abstract class GWebCacheRequest : IRequestValidator {
	public abstract bool IsValidRequest();
}
