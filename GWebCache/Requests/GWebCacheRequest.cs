namespace GWebCache.Requests{
	/// <summary>
	/// Base class for all GWebCache requests.
	/// </summary>
	public abstract class GWebCacheRequest {
		internal abstract bool IsValidRequest();
	}
}