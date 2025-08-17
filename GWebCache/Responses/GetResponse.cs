using System;
using System.Collections.Generic;
using System.Linq;
using GWebCache.Extensions;
using GWebCache.Models;
using System.Net;
using System.Net.Http;
using GWebCache.Exceptions;

namespace GWebCache.Responses{
	/// <summary>
	/// A response class containing the Nodes and GWebCache valid for V2 compliant GWebCaches.
	/// </summary>
	/// <remarks>This is basically the combination of a <see cref="UrlFileResponse"/> and <see cref="UrlFileResponse"/></remarks>
	public  class GetResponse : GWebCacheResponse {

		/// <summary>
		/// List of know gnutella nodes on the network
		/// </summary>
		public List<GnutellaNode> GnutellaNodes { get; }  = new List<GnutellaNode>();

		/// <summary>
		/// List of other GWebCaches
		/// </summary>
		public List<GWebCacheNode> WebCacheNodes { get; } = new List<GWebCacheNode>();

		/// <summary>
		/// A message is valid if it complies with <see cref="GWebCacheResponse.IsValidResponse(HttpResponseMessage?)"/> 
		/// and the content of the body can't start with error or i (indicating a warning or an error)
		/// </summary>
		/// <param name="responseMessage">The HTTP response returned from the request</param>
		/// <returns>Boolean indicating if the response can be parsed</returns>
		internal override bool IsValidResponse(HttpResponseMessage responseMessage) {
			if (!base.IsValidResponse(responseMessage))
				return false;

			string content = responseMessage!.ContentAsString();
			return !content.Contains("error", StringComparison.InvariantCultureIgnoreCase)
				|| content.StartsWith("i", StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Check if message complies with <see cref="GWebCacheResponse.IsValidV2Response(HttpResponseMessage?)"/>
		/// </summary>
		internal override bool IsValidV2Response(HttpResponseMessage responseMessage) {
			if(!base.IsValidV2Response(responseMessage))
				return false;

			return true;
		}

		/// <summary>
		/// Not relevant, Get Responses are only returned by GWebCaches following the V2 specification
		/// </summary>
		/// <param name="response">The HTTP response returned from the request</param>
		/// <exception cref="VersionMismatchException">Will always be thrown</exception>
		internal override void Parse(HttpResponseMessage response) {
			throw new VersionMismatchException();
		}

		/// <summary>
		/// Parses the HTTP Response message into a get response.
		/// </summary>
		/// <param name="response">The HTTP response returned from the request</param>
		internal override void ParseV2(HttpResponseMessage response) {
			string[] lines = response.ContentAsString().Split("\n").Select(l => l.Trim()).Where(l => !string.IsNullOrEmpty(l)).ToArray();
			foreach(string line in lines) {
				string[] fields = line.Split("|");

				if (fields.Length < 2)
					continue;

				//if the line starts with u it's a node, fields are | seperated
				if (fields[0].Equals("u",StringComparison.InvariantCultureIgnoreCase) 
					&& Uri.TryCreate(fields[1], UriKind.Absolute, out Uri uri)) {
					GWebCacheNode gWebCacheNode = new GWebCacheNode(uri);

					if (fields.Length >= 3 && double.TryParse(fields[2], out double seconds))
						gWebCacheNode.ActiveSince = TimeSpan.FromSeconds(seconds);

					WebCacheNodes.Add(gWebCacheNode);
				}
				else if (fields[0].Equals("h", StringComparison.InvariantCultureIgnoreCase)) {
					string[] nodeParts = fields[1].Split(':');
					if (nodeParts.Length < 2 || 
						!IPAddress.TryParse(nodeParts[0], out IPAddress iPAddress) || 
						!int.TryParse(nodeParts[1],out int port) || port<=0)
						continue;

					GnutellaNode node = new GnutellaNode(iPAddress, port);

					if (fields.Length >= 3 && double.TryParse(fields[2], out double seconds))
						node.ActiveSince = TimeSpan.FromSeconds(seconds);

					GnutellaNodes.Add(node);
				}
			}
		}
	}
}