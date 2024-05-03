using GWebCache;
using GWebCache.Models.Enums;
using GWebCache.ReponseProcessing;
using GWebCache.Reponses;
using GWebCache.Requests;
namespace GWebCacheTest;

internal class Program {
	static void Main(string[] args) {

		//v1 cache
		IGWebCacheClient client = new GWebCacheClient("http://gweb3.4octets.co.uk/gwc.php");
		Result<PongResponse> pingResponse = client.Ping();
		Result<UrlFileResponse> urlFileResponse = client.GetUrlFile();
		Result<HostfileResponse> hostFileResponse  = client.GetHostfile();
		//V2 cache
		IGWebCacheClient clientV2 = new GWebCacheClient("http://www.k33bz.com/g2/bazooka.php");
		Result<GetResponse> response = clientV2.Get(GnutellaNetwork.Gnutella2);
		Result<HostfileResponse> response2 = clientV2.GetHostfile(GnutellaNetwork.Gnutella2);
		Result<UrlFileResponse> response3 = clientV2.GetUrlFile(GnutellaNetwork.Gnutella2);
		UpdateRequest updateRequest = new() { GnutellaNode = new GWebCache.Models.GnutellaNode("41.96.246.173", 43624), Network = GnutellaNetwork.Gnutella2 };
		Result<UpdateResponse> response4 = clientV2.Update(updateRequest);
	}
}
