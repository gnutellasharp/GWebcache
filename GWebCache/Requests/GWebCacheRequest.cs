namespace GWebCache.Requests;

/// <summary>
/// Base class for all GWebCache requests.
/// </summary>
/// <see cref="IRequestValidator"/>
public abstract class GWebCacheRequest {
	internal abstract bool IsValidRequest();
}
