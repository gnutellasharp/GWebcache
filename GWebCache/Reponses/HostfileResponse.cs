using GWebCache.Models;
using GWebCache.Extensions;

namespace GWebCache.Reponses;

public class HostfileResponse : GWebCacheResponse {
	public List<GnutellaNode> HostfileLines { get; set; } = new List<GnutellaNode>();

	internal override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;

		// If the response contains the word "error" then it is not a valid response
		string content = responseMessage!.ContentAsString();
		return content.Contains("error", StringComparison.InvariantCultureIgnoreCase);
	}

	internal override void Parse(HttpResponseMessage response) {
		string content = response.Content?.ReadAsStringAsync().Result ?? "";
		string[] lines = content.Split("\n").Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();

		foreach (string line in lines) {
			string[] parts = line.Split(":");
			if (parts.Length == 2) {
				HostfileLines.Add(new GnutellaNode(parts[0], int.Parse(parts[1])));
			}
		}
	}

	internal override void ParseV2(HttpResponseMessage response) {
		throw new NotImplementedException();
	}
}
