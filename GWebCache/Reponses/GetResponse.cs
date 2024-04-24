using GWebCache.Extensions;
using GWebCache.Models;
using System.Net;

namespace GWebCache.Reponses;

public  class GetResponse : GWebCacheResponse {
	public List<GnutellaNode> Nodes { get; set; } = [];
	public List<GWebCacheNode> WebCacheNodes { get; set; } = [];

	internal override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;

		string content = responseMessage!.ContentAsString();
		return !content.Contains("error", StringComparison.InvariantCultureIgnoreCase)
			|| content.StartsWith("i", StringComparison.InvariantCultureIgnoreCase);
	}

	internal override bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		if(!base.IsValidV2Response(responseMessage))
			return false;

		return true;
	}

	internal override void Parse(HttpResponseMessage response) {
		throw new NotImplementedException();
	}

	internal override void ParseV2(HttpResponseMessage response) {
		string[] lines = response.ContentAsString().Split("\n").Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
		foreach(string line in lines) {
			string[] fields = [..line.Split("|")];

			if (fields.Length < 2)
				continue;

			if (fields[0].Equals("u",StringComparison.InvariantCultureIgnoreCase) 
				&& Uri.TryCreate(fields[1], UriKind.Absolute, out Uri? uri)) {
				GWebCacheNode webcache = new GWebCacheNode(uri);

				if (fields.Length >= 3 && double.TryParse(fields[2], out double seconds))
					webcache.ActiveSince = TimeSpan.FromSeconds(seconds);

				WebCacheNodes.Add(webcache);
			}
			else if (fields[0].Equals("h", StringComparison.InvariantCultureIgnoreCase)) {
				string[] nodeParts = fields[1].Split(':');
				if (nodeParts.Length < 2 || 
					!IPAddress.TryParse(nodeParts[0], out IPAddress? iPAddress) || 
					!int.TryParse(nodeParts[1],out int port))
					continue;

				GnutellaNode node = new(iPAddress, port);

				if (fields.Length >= 3 && double.TryParse(fields[2], out double seconds))
					node.ActiveSince = TimeSpan.FromSeconds(seconds);

				Nodes.Add(node);
			}
		}
	}
}
