using GWebCache.Models;
using System.Net;

namespace GWebCache.Reponses;

public class HostfileResponse : GWebCacheResponse {
	public List<GnutellaNode> HostfileLines { get; set; } = new List<GnutellaNode>();

	public override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		bool response = base.IsValidResponse(responseMessage);
		string? content = responseMessage?.Content?.ReadAsStringAsync().Result;
		return response && !string.IsNullOrWhiteSpace(content) && !content.Contains("error", StringComparison.InvariantCultureIgnoreCase);
	}
	public override void Parse(HttpResponseMessage response) {
		string content = response.Content?.ReadAsStringAsync().Result ?? "";
		string[] lines = content.Split("\n").Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();

		foreach (string line in lines) {
			string[] parts = line.Split(":");
			if (parts.Length == 2) {
				HostfileLines.Add(new GnutellaNode {
					IPAddress = IPAddress.Parse(parts[0]),
					port = int.Parse(parts[1])
				});
			}
		}
	}
}
