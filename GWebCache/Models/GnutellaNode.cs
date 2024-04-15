using System.Net;
using System.Web;

namespace GWebCache.Models;

public class GnutellaNode {
	public IPAddress? IPAddress { get; set; }
	public int Port { get; set; }

	public GnutellaNode(string ipAddress, int port) {
		IPAddress.TryParse(ipAddress, out IPAddress? ip);
		IPAddress = ip;
		Port = port;
	}

	public GnutellaNode(IPAddress IPAddress, int port) {
		this.IPAddress = IPAddress;
		Port = port;
	}

	override public string ToString() {
		return HttpUtility.UrlEncode($"{IPAddress}:{Port}");
	}

	override public bool Equals(object? obj) {
		if (obj != null && obj is GnutellaNode node)
			return node.IPAddress == IPAddress && node.Port == Port;

		return false;
	}

	public override int GetHashCode() {
		throw new NotImplementedException();
	}
}