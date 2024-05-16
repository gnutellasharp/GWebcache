using GWebCache.Models.Enums;

namespace GWebCache.Test.Mock_caches;
internal class BeaconCache : IMockCache {
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
			"U|http://test.net/g2/bazooka.php|31124\r\n" +
			"U|http://test.net:3558/|83716\r\n" +
			"I|access|period|3300";
	}

	public string GetHostfileResponse() {
		return "127.0.218.247:37937\r\n" +
			"127.190.57.33:15750\r\n" +
			"127.191.191.250:36594\r\n" +
			"127.67.13.167:43586\r\n" +
			"127.202.152.57:5786\r\n";
	}

	public string[] GetHosts() {
		return ["127.0.218.247:37937", "127.190.57.33:15750", "127.191.191.250:36594", "127.67.13.167:43586", "127.202.152.57:5786"];
	}

	public string GetPongRespone() {
		return "PONG Beacon Cache II 0.8.0.1";
	}

	public string GetStatFileResponse() {
		return
			"784569\r\n" +
			"11\r\n" +
			"2";
	}


	public bool SupportsV1() {
		return true;
	}

	public string[] GetSupportedNetworks() {
		return [];
	}

	public string GetUpdateReponse(GnutellaNetwork? net) { 
		return "I | OK ";
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
		return "Beacon Cache II 0.8.0.1";
	}

	public bool IsV2Cache() {
		return false;
	}

	public bool SupportsStats() {
		return true;
	}

	public int GetTotalNumberOfRequests() {
		return 784569;
	}

	public int GetNumberOfRequestsInLastHour() {
		return 11;
	}

	public int GetNumberOfUpdatesInLastHour() {
		return 2;
	}

	public string GetUpdateMessage() {
		return "";
	}

	public bool UpdateCallSucceeeded() {
		return true;
	}
}
