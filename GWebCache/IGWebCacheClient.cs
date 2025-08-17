using GWebCache.Models.Enums;
using GWebCache.ResponseProcessing;
using GWebCache.Responses;
using GWebCache.Requests;
using GWebCache.Client;
using GWebCache.Models;

namespace GWebCache{
	/// <summary>
	/// Client for interacting with a GWebCache server. Supports Version 1 and 2 of the protocol.
	/// </summary>
	public interface IGWebCacheClient {
		/// <summary>
		/// Preforms a ping request to the server to check if it is alive.
		/// </summary>
		/// <returns>Boolean indicating if the server successfully answered the ping request</returns>
		/// <remarks>A successful pong response is defined in <see cref="PongResponse"/></remarks>
		/// <see cref="Ping"/>
		/// <seealso cref="PongResponse"/>
		bool CheckIfAlive();

		/// <summary>
		/// Returns the property <see cref="GWebCacheClientConfig.IsV2"/>
		/// </summary>
		/// <remarks>If this property wasn't specified, a ping request is made when creating the client to fill in this property.</remarks>
		/// <see cref="GWebCacheClientConfig.IsV2"/>
		/// <seealso cref="Ping"/>
		bool WebCacheIsV2();

		/// <summary>
		/// Preforms a ping request to the GWebCache. Mainly used to check if the server is alive.
		/// </summary>
		/// <see cref="Result{T}"/>
		/// <seealso cref="PongResponse"/>
		Result<PongResponse> Ping();

		/// <summary>
		/// Returns the stats of the GWebCache server. 
		/// </summary>
		/// <remarks>Not all GWebCache servers actually implement this. So don't assume you will get a successful result.</remarks>
		/// <see cref="Result{T}"/>
		/// <seealso cref="StatFileResponse"/>
		Result<StatFileResponse> GetStats();

		/// <summary>
		/// Retrieves a list of Gnutella Nodes from the GWebCache. 
		/// </summary>
		/// <param name="network">
		/// The network you want to get the nodes from.
		/// Will not be specified by default.
		/// </param>
		/// <remarks>
		/// V1 caches typically don't include gnutella 2 or the network parameter.
		/// However, they will ignore it so it's always best to specify.
		/// </remarks>
		/// <remarks>
		/// You can use this method on both versions. 
		/// It is however recommended to use the <see cref="Get(GnutellaNetwork?)"/> method for version 2 of the specification.
		/// </remarks>
		/// <example>
		/// <code>
		/// IGWebCacheClient client = new GWebCacheClient("url");
		/// client.GetHostFile(GnutellaNetwork.Gnutella2);
		/// </code>
		/// </example>
		/// <see cref="Result{T}"/>
		/// <seealso cref="HostFileResponse"/>
		/// <seealso cref="GnutellaNetwork"/>
		/// <seealso cref="Get(GnutellaNetwork?)"/>
		/// <seealso cref="GnutellaNode"/>
		Result<HostFileResponse> GetHostFile(GnutellaNetwork? network = null);

		/// <summary>
		/// Retrieves a list of Urls to other GWebCaches.
		/// </summary>
		/// <param name="network">
		/// The network you want to get the nodes from.
		/// Will not be specified by default.
		/// </param>
		/// <remarks>
		/// While the network parameter doesn't make a difference in V2 caches a lot of them require it and will return the same result regardless.
		/// </remarks>
		/// <example>
		/// <code>
		/// IGWebCacheClient client = new GWebCacheClient("url");
		/// client.GetUrlFile(GnutellaNetwork.Gnutella2);
		/// </code>
		/// </example>
		/// <see cref="UrlFileResponse"/>
		/// <seealso cref="Result{T}"/>
		/// <seealso cref="GnutellaNetwork"/>
		/// <seealso cref="GWebCacheNode"/>
		Result<UrlFileResponse> GetUrlFile(GnutellaNetwork? network = null);


		/// <summary>
		/// Retrieves a list of Gnutella Nodes and GWebCaches urls from a V2 compliant GWebCache. This is only valid on a V2 cache! 
		/// </summary>
		/// <param name="network">The network you want to get the nodes and urls from.</param>
		/// <see cref="GetResponse"/>
		/// <seealso cref="Result{T}"/>
		/// <seealso cref="GnutellaNetwork"/>
		/// <seealso cref="GWebCacheNode"/>
		Result<GetResponse> Get(GnutellaNetwork? network);

		/// <summary>
		/// Sends an Update request to the GWebCache. Indicating a new gnutella node, or GWebCache urls or both.
		/// </summary>
		/// <remarks>Note that there's a network parameter in the update request.</remarks>
		/// <remarks>The update response might also contain warnings when for example you're being rate limited.</remarks>
		/// <example>
		/// <code>
		/// IGWebCacheClient client = new GWebCacheClient("V2 url");
		/// GnutellaNode node = GnutellaNode("ip", port);
		/// GWebCacheNode webCache = new GWebCacheNode("url");
		/// UpdateRequest updateRequest = new UpdateRequest() {
		/// GnutellaNode = node, 
		/// GWebCacheNode = webCache,
		/// Network = GnutellaNetwork.Gnutella2 
		/// };
		/// client.Update(updateRequest);
		/// </code>
		/// </example>
		/// <see cref="UpdateResponse"/>"
		/// <seealso cref="Result{T}"/>
		/// <seealso cref="UpdateRequest"/>
		/// <seealso cref="GnutellaNode"/>
		/// <seealso cref="GWebCacheNode"/>
		Result<UpdateResponse> Update(UpdateRequest updateRequest);
	}
}