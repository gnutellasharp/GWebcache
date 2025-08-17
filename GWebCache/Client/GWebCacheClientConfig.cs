namespace GWebCache.Client{
	/// <summary>
	/// Configuration for when preforming requests to the WebCache.
	/// Used by <see cref="GWebCacheClient"/>.
	/// </summary>
	public class GWebCacheClientConfig {
		/// <value>A string (recommended 4 characters) to identify which client is interacting with the GWebCache.</value>
		public string ClientName { get; set; }
		/// <value>The version of the client.</value>
		public string Version { get; set; }
		/// <value>While not needed according to the specification some GWebCaches block requests that don't provide a user agent.</value>
		public string UserAgent => $"{ClientName}/{Version}";
		/// <value>Indicates if the GWebCache is a V2 cache.</value>
		public bool? IsV2 { get; set; }
		/// <value>Default configuration with the client name set to CSGN and version number</value>
		public static GWebCacheClientConfig Default => new GWebCacheClientConfig() { ClientName = "CSGN", Version = "0.1.0" };
	}
}