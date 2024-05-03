using GWebCache.Models;
using GWebCache.Extensions;

namespace GWebCache.Reponses;


/// <summary>
/// A response class containing the Nodes known to the webcache.
/// </summary>
/// <remarks>For a V2 compliant webcache the <see cref="GetResponse"/> is being used</remarks>
public class HostfileResponse : GWebCacheResponse {

	/// <summary>
	/// The list of nodes known to the webcache
	/// </summary>
	public List<GnutellaNode> GnutellaNodes { get; set; } = new List<GnutellaNode>();


	/// <summary>
	/// A message is valid if it complies with <see cref="GWebCacheResponse.IsValidResponse(HttpResponseMessage?)"/> 
	/// and the content doesn't start with error
	/// </summary>
	/// <param name="responseMessage">The HTTP response returned from the request</param>
	/// <returns>Boolean indicating if the webresponse can be parsed</returns>
	internal override bool IsValidResponse(HttpResponseMessage? responseMessage) {
		if (!base.IsValidResponse(responseMessage))
			return false;

		// If the response contains the word "error" then it is not a valid response
		string content = responseMessage!.ContentAsString();
		return !content.Contains("error", StringComparison.InvariantCultureIgnoreCase);
	}

	/// <summary>
	/// Parses the HTTP response from the server into a list of gnutella nodes
	/// </summary>
	/// <param name="response">The HTTP response from the server</param>
	internal override void Parse(HttpResponseMessage response) {
		string content = response.Content?.ReadAsStringAsync().Result ?? "";
		string[] lines = content.Split("\n").Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();

		foreach (string line in lines) {
			string[] parts = line.Split(":");
			if (parts.Length == 2) {
				GnutellaNodes.Add(new GnutellaNode(parts[0], int.Parse(parts[1])));
			}
		}
	}

	/// <summary>
	/// Is never V2 so will return false. <see cref="GetResponse"/>
	/// </summary>
	internal override bool IsValidV2Response(HttpResponseMessage? responseMessage) {
		return false;
	}

	/// <summary>
	/// Not used, internally the <see cref="GetResponse"/> is used for forwards compatibility
	/// </summary>
	/// <param name="response">The HTTP response from the server</param>
	/// <exception cref="NotImplementedException">Will alwats be thrown</exception>
	internal override void ParseV2(HttpResponseMessage response) {
		throw new NotImplementedException();
	}
}
