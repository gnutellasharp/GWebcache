namespace GWebCache.Client;

public class GWebCacheClientConfig {
	public string? ClientName { get; set; }
	public string? Version { get; set; }
	public string UserAgent => $"{ClientName}/{Version}";
	public static GWebCacheClientConfig Default => new() { ClientName = "CSGN", Version = "0.0.2" };
}
