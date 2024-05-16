using GWebCache.Models.Enums;
//http://dkac.trillinux.org/dkac/dkac.php
namespace GWebCache.Test.Mock_caches;
internal class SkullsCache : IMockCache {
	public string GetGetResponse(GnutellaNetwork? net) {
		return
	"H|127.0.218.247:37937|48419\r\n" +
	"H|127.190.57.33:15750|41184\r\n" +
	"H|127.191.191.250:36594|45329\r\n" +
	"H|127.67.13.167:43586|15199\r\n" +
	"H|127.202.152.57:5786|21781\r\n" +
	"U|http://test.net/skulls.php|35098\r\n" +
	"U|http://test.net:3558/|41992\r\n" +
	"U|http://test.net/skulls.php|76603\r\n" +
	"U|http://test.net/g2/bazooka.php|31124\r\n";
	}

	public string GetHostfileResponse() {
		return
			"127.0.218.247:37937\r\n" +
			"127.190.57.33:15750\r\n" +
			"127.191.191.250:36594\r\n" +
			"127.67.13.167:43586\r\n" +
			"127.202.152.57:5786\r\n";
	}
	public string[] GetHosts() {
		return ["127.0.218.247:37937", "127.190.57.33:15750", "127.191.191.250:36594", "127.67.13.167:43586", "127.202.152.57:5786"];
	}


	public string GetPongRespone() {
		return "PONG Skulls 0.3.6\r\n" +
			"I|pong|Skulls 0.3.6|gnutella2-gnutella-mute-antsnet-pastella-kad-foxy";
	}

	public string GetStatFileResponse() {
		return "8896543\r\n" +
			"46\r\n" +
			"5";
	}

	public bool SupportsV1() {
		return true;
	}

	public string[] GetSupportedNetworks() {
		return ["gnutella2", "gnutella", "mute", "antsnet", "pastella", "kad", "foxy"];
	}

	public string GetUpdateReponse(GnutellaNetwork? net){ 
		return "I|update|OK|URL already updated\r\n";
	}

	public string GetUrlfileResponse() {
		return
		"http://test.net/skulls.php\n" +
		"http://test.net:3558/\r\n" +
		"http://test.net/skulls.php\r\n" +
		"http://test.net/g2/bazooka.php\r\n";
	}

	public string[] GetUrls() {
		return ["http://test.net/skulls.php", "http://test.net:3558/", "http://test.net/skulls.php", "http://test.net/g2/bazooka.php"];
	}

	public string GetVersion() {
		return "Skulls 0.3.6";
	}

	public bool IsV2Cache() {
		return true;
	}

	public bool SupportsStats() {
		return true;
	}

	public int GetTotalNumberOfRequests() {
		return 8896543;
	}

	public int GetNumberOfRequestsInLastHour() {
		return 46;
	}

	public int GetNumberOfUpdatesInLastHour() {
		return 5;
	}

	public string GetUpdateMessage() {
		return "URL already updated";
	}

	public bool UpdateCallSucceeeded() {
		return true;
	}
}
