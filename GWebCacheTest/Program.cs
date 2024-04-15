using GWebCache;
using GWebCache.ReponseProcessing;
using GWebCache.Reponses;
using GWebCache.Requests;
namespace GWebCacheTest;

internal class Program {
	static void Main(string[] args) {

		//v1 cache
		IGWebCacheClient client = new GWebCacheClient("http://gweb3.4octets.co.uk/gwc.php");
		Result<PongResponse> pingResponse = client.Ping();
		//V2 cache
		IGWebCacheClient clientV2 = new GWebCacheClient("http://www.k33bz.com/g2/bazooka.php");
		pingResponse = clientV2.Ping();
	}
}
