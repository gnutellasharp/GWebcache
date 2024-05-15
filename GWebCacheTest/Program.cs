using GWebCache;
using GWebCache.Models.Enums;
using GWebCache.ReponseProcessing;
using GWebCache.Reponses;
using GWebCache.Requests;
namespace GWebCacheTest;

internal class Program {
	static void Main(string[] args) {
		string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "discoveredurls.txt");
		if (File.Exists(filePath))
			File.Delete(filePath);

		File.Create(filePath).Close();

		StreamWriter sw = new StreamWriter(File.Open(filePath, FileMode.Open));
		sw.AutoFlush = true;

		IGWebCacheClient client;
		List<string> discovered = new List<string>();

		Stack<string> toInvestigate = new Stack<string>();
		toInvestigate.Push("http://gweb3.4octets.co.uk/gwc.php");

		do {
			try {
				string url = toInvestigate.Pop();

				if (discovered.Contains(url))
					continue;

				discovered.Add(url);
				client = new GWebCacheClient(url);

				Result<UrlFileResponse> result = client.GetUrlFile(GnutellaNetwork.Gnutella2);

				if (result.WasSuccessful) {
					sw.WriteLine(url);
					result.ResultObject.WebCacheNodes.ForEach(u => {

						toInvestigate.Push(u.Url?.ToString() ?? "");
					});
				} else { 
					sw.WriteLine($"Failed to get url file: {url} for {result.ErrorMessage}");
				}
			} catch {
				continue;
			}
		} while (toInvestigate.Any());
		sw.Close();
		//	//v1 cache
		//	IGWebCacheClient client = new GWebCacheClient("http://gweb3.4octets.co.uk/gwc.php");
		//	Result<PongResponse> pingResponse = client.Ping();
		//	Result<UrlFileResponse> urlFileResponse = client.GetUrlFile();
		//	Result<HostfileResponse> hostFileResponse  = client.GetHostfile();
		//	//V2 cache
		//	IGWebCacheClient clientV2 = new GWebCacheClient("http://www.k33bz.com/g2/bazooka.php");
		//	Result<GetResponse> response = clientV2.Get(GnutellaNetwork.Gnutella2);
		//	Result<HostfileResponse> response2 = clientV2.GetHostfile(GnutellaNetwork.Gnutella2);
		//	Result<UrlFileResponse> response3 = clientV2.GetUrlFile(GnutellaNetwork.Gnutella2);
		//	UpdateRequest updateRequest = new() { GnutellaNode = new GWebCache.Models.GnutellaNode("41.96.246.173", 43624), Network = GnutellaNetwork.Gnutella2 };
		//	Result<UpdateResponse> response4 = clientV2.Update(updateRequest);
	}
}
