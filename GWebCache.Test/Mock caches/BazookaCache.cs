using GWebCache.Models.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GWebCache.Test.Mock_caches;
internal class BazookaCache : IMockCache {
	public string GetGetResponse(GnutellaNetwork? net) {
		string result = "ERROR Unknown NOT Supported.";
		 switch (net) {
			case GnutellaNetwork.Gnutella:
				result = "ERROR gnutella NOT Supported.";
				break;
			case GnutellaNetwork.Gnutella2:
				result =
			"h|127.0.218.247:37937|48419\r\n" +
			"h|127.190.57.33:15750|41184\r\n" +
			"h|127.191.191.250:36594|45329\r\n" +
			"h|127.67.13.167:43586|15199\r\n" +
			"h|127.202.152.57:5786|21781\r\n" +
			"u|http://test.net/skulls.php|35098\r\n" +
			"u|http://test.net/skulls.php|76603\r\n" +
			"u|http://test.net/g2/bazooka.php|31124\r\n" +
			"u|http://test.net:3558/|83716\r\n";
			break;
		}

		return result;
	}

	public string GetHostfileResponse() {
		return "ERROR Invalid Command";
	}

	public string[] GetHosts() {
		return ["127.0.218.247:37937", "127.190.57.33:15750", "127.191.191.250:36594", "127.67.13.167:43586", "127.202.152.57:5786"];
	}

	public string GetPongRespone() {
		return "i|pong|Bazooka 0.3.6b|Gnutella2";
	}

	public string GetStatFileResponse() {
		return "ERROR Invalid Command";
	}

	public string[] GetSupportedNetworks() {
		return ["Gnutella2"];
	}

	public string GetUpdateReponse(GnutellaNetwork? net) {
		return "i|update|OK";
	}

	public string GetUrlfileResponse() {
		return "ERROR Invalid Command";
	}

	public string[] GetUrls() {
		return ["http://test.net/skulls.php", "http://test.net/skulls.php", "http://test.net/g2/bazooka.php", "http://test.net:3558/"];
	}


	public string GetVersion() {
		return "Bazooka 0.3.6b";
	}

	public bool IsV2Cache() {
		return true;
	}

	public bool SupportsV1() {
		return false;
	}
}
