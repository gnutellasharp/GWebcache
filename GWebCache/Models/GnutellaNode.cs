using System.Net;
using System.Web;

namespace GWebCache.Models;

/// <summary>
/// The model representing a Gnutella node on the network stored in the cache.
/// </summary>
public class GnutellaNode {

	/// <summary>
	/// The ip adress of the node
	/// </summary>
	public IPAddress? IPAddress { get; set; }

	/// <summary>
	/// Listening port of the node
	/// </summary>
	public int Port { get; set; }

	/// <summary>
	/// When did the webcache get an update about this node.
	/// </summary>
	/// <remarks>This is exclusively provided by V2 webcaches so always check if this is not filled in</remarks>
	public TimeSpan ActiveSince { get; set; }

	/// <summary>
	/// Constructs a new Gnutella Node
	/// </summary>
	/// <param name="ipAddress">string representation of the IP</param>
	/// <param name="port">Listening port of the node</param>
	public GnutellaNode(string ipAddress, int port) {
		_ = IPAddress.TryParse(ipAddress, out IPAddress? ip);
		IPAddress = ip;
		Port = port;
	}

	/// <summary>
	/// Constructs a new Gnutella Node
	/// </summary>
	/// <param name="ipAddress">IPaddress of the node</param>
	/// <param name="port">Listening port of the node</param>
	public GnutellaNode(IPAddress IPAddress, int port) {
		this.IPAddress = IPAddress;
		Port = port;
	}

	/// <summary>
	/// Returns the node as ip:port and url encodes it.
	/// </summary>
	/// <remarks>The reason for the url encoding is that we can then immeaditely send it on to the webcache</remarks>
	override public string ToString() {
		return HttpUtility.UrlEncode($"{IPAddress}:{Port}");
	}

	/// <summary>
	/// Two nodes are equal if their ips are the same and their ports are the same.
	/// </summary>
	/// <returns>An indication if two nodes are equal</returns>
	override public bool Equals(object? obj) {
		if (obj != null && obj is GnutellaNode node)
			return node.IPAddress == IPAddress && node.Port == Port;

		return false;
	}

	/// <summary>
	/// Not relevant since the Equals method doesn't need it
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	public override int GetHashCode() {
		throw new NotImplementedException();
	}
}