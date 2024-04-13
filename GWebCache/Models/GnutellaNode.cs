using System.Net;

namespace GWebCache.Models;

public class GnutellaNode {
	public IPAddress? IPAddress { get; set; }
	public int port { get; set; }
}