namespace GWebCache.Client;

/// <summary>
/// Configuration for when preforming reuqests to the WebCache.
/// Used by <see cref="GWebCacheClient"/>.
/// </summary>
public class GWebCacheClientConfig {
	/// <value>A string (recommended 4 characters) to identify which client is interacting with the WebCache.</value>
	public string? ClientName { get; set; }
	/// <value>The version of the client.</value>
	public string? Version { get; set; }
	/// <value>While not needed according to the specification some webcaches block requests that don't provide a user agent.</value>
	public string UserAgent => $"{ClientName}/{Version}";
	/// <value>Indicates if the webcache is a V2 cache.</value>
	public bool? IsV2 { get; set; }
	/// <value>Default configuration (clientname = CSGN) and version number</value>
	public static GWebCacheClientConfig Default => new() { ClientName = "CSGN", Version = "0.1.0" };
}
