using System;
using System.Net;

namespace GWebCache.Models{
	/// <summary>
	/// The model representing a Gnutella node on the network stored in the cache.
	/// </summary>
	public class GnutellaNode {

		/// <summary>
		/// The ip address of the node
		/// </summary>
		// ReSharper disable once MemberCanBePrivate.Global
		public IPAddress IpAddress { get; }

		/// <summary>
		/// Listening port of the node
		/// </summary>
		public int Port { get; }

		/// <summary>
		/// When did the GWebCache get an update about this node.
		/// </summary>
		/// <remarks>This is exclusively provided by V2 GWebCaches so always check if this is not filled in</remarks>
		public TimeSpan ActiveSince { get; set; }

		/// <summary>
		/// Constructs a new Gnutella Node
		/// </summary>
		/// <param name="ipAddress">string representation of the IP</param>
		/// <param name="port">Listening port of the node</param>
		/// <exception cref="ArgumentException">If the IP-address is not valid or the port number is below zero</exception>
		public GnutellaNode(string ipAddress, int port) {
			if (port <= 0)
				throw new ArgumentException("Port can't be negative");

			bool parsed = IPAddress.TryParse(ipAddress, out IPAddress ip);
			if (!parsed || ip == null)
				throw new ArgumentException("Invalid IP address");

			this.IpAddress = ip;
			this.Port = port;
		}

		/// <summary>
		/// Constructs a new Gnutella Node
		/// </summary>
		/// <param name="ipAddress">IP-address of the node</param>
		/// <param name="port">Listening port of the node</param>
		/// <exception cref="ArgumentException">If the port number is below zero</exception>
		public GnutellaNode(IPAddress ipAddress, int port) {
			if (port <= 0)
				throw new ArgumentException("Port can't be negative");

			this.IpAddress = ipAddress;
			Port = port;
		}

		/// <summary>
		/// Returns the node as ip:port and url encodes it.
		/// </summary>
		/// <remarks>The reason for the url encoding is that we can then immediately send it on to the GWebCache</remarks>
		public override string ToString() {
			return $"{this.IpAddress}:{Port}";
		}

		/// <summary>
		/// Two nodes are equal if their ips are the same and their ports are the same.
		/// </summary>
		/// <returns>An indication if two nodes are equal</returns>
		public override bool Equals(object obj) {
			if (obj is GnutellaNode node)
				return node.IpAddress!.Equals(this.IpAddress) && node.Port == Port;

			return false;
		}

		/// <summary>
		/// Not relevant since the Equals method doesn't need it
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public override int GetHashCode() {
			return this.IpAddress.GetHashCode() + this.Port.GetHashCode();
		}
	}
}