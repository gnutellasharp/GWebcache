using GWebCache.Models.Enums;

namespace GWebCache.Test.Mock_caches;
internal class DKACCache : IMockCache {
	public string GetGetResponse(GnutellaNetwork? net) {
		if (net == null)
			return "ERROR No network";

		return "h|127.0.218.247:37937|48419\r\n" +
			"h|127.190.57.33:15750|41184\r\n" +
			"h|127.191.191.250:36594|45329\r\n" +
			"h|127.67.13.167:43586|15199\r\n" +
			"h|127.202.152.57:5786|21781\r\n" +
			"u|http://test.net/skulls.php|35098\r\n" +
			"u|http://test.net:3558/|41992\r\n" +
			"u|http://test.net/skulls.php|76603\r\n" +
			"u|http://test.net/g2/bazooka.php|31124\r\n";
	}

	public string[] GetHosts() {
		return ["127.0.218.247:37937", "127.190.57.33:15750", "127.191.191.250:36594", "127.67.13.167:43586", "127.202.152.57:5786"];
	}

	public string GetHostfileResponse() {
		return "This is DKAC/Enticing-Enumon. Source\r\n";
	}

	public string GetPongRespone() {
		return "i|pong|DKAC/Enticing-Enumon";
	}

	public string GetStatFileResponse() {
		return "This is DKAC/Enticing-Enumon. Source\r\n";
	}

	public string[] GetSupportedNetworks() {
		return [];
	}

	public string GetUpdateReponse(GnutellaNetwork? net) {
		string result = "ERROR No network";
		switch (net) {
			case GnutellaNetwork.Gnutella:
				result = "ERROR unsupported network";
				break;
			case GnutellaNetwork.Gnutella2:
				result = "i|update|OK\r\n";
				break;
		}
		return result;
	}

	public string GetUrlfileResponse() {
		return "This is DKAC/Enticing-Enumon. Source\r\n";
	}

	public string GetVersion() {
		return "DKAC/Enticing-Enumon";
	}

	public bool IsV2Cache() {
		return true;
	}


	public bool SupportsV1() {
		return false;
	}

	public string[] GetUrls() {
		return ["http://test.net/skulls.php", "http://test.net:3558/", "http://test.net/skulls.php", "http://test.net/g2/bazooka.php"];
	}

}
