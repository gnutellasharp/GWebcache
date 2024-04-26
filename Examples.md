# Examples

## Querying a v1 cache

```C#
//make a client for interacting with the webcache
IGWebCacheClient client = new GWebCacheClient("Webcache url V1");
//if the client failed to respond to the ping correctly you can't query it
if(!client.CheckIfAlive())
    return;

//ask the cache for gnutella nodes
Result<HostfileResponse> hostfileResponse = client.GetHostfile(GnutellaNetwork.Gnutella2);
//ask the cache for other webcache urls
Result<UrlFileResponse> urlFileResponse = client.GetUrlFile(GnutellaNetwork.Gnutella2);
```

## Error handling

We use  a so called result object. This is basically a wrapper around the response that handles the cases where the webcache returns an error (be it a bad request or a bad response)

```c#
//assuming the same setup as the querrying example above
Result<UrlFileResponse> urlFileResponse = client.GetUrlFile();
if(!urlFileResponse.WasSuccessful){
	throw new Exception(urlFileResponse.ErrorMessage);
}
UrlFileResponse responseObject = urlFileResponse.ResultObject;
List<string> urls = responseObject.WebCaches.Select(cache=>cache.Url).ToList();
```

## Important models

The two main models you'll be working with are caches and gnutella nodes. They're described more in detail in the documentation but here's the main properties

```{C#}
GnutellaNode node = new GnutellaNode("127.0.0.1", 1234);
Console.WriteLine($"{node.IPAddress}:{node.Port}");

GWebCacheNode gWebCacheNode = new GWebCacheNode("http://examplecache.org/test.php");
Console.WriteLine(gWebCacheNode.Url);
```

## Querying a V2 cache

The querying principle is a bit different. A V2 cache does not split up the hostfile and the urlfile. Combining it in one call. So it's more efficient to use the get call if you need both properties. Mind you that we've also made the **hostfile and urlfile calls are <u>forwards-compatible</u>.**

Another difference is the fact that a V2 cache might support different networks so you will have to provide a network parameter. Note that you can also provide this parameter to a V1 cache this will be ignored.

```c#
Result<GetResponse> getResponse = clientV2.Get(GnutellaNetwork.Gnutella2);
if (!getResponse.WasSuccessful)
	return;

GetResponse responseObject = getResponse.ResultObject!;
IEnumerable<Uri?> urls = responseObject.WebCacheNodes.Select(cache => cache.Url);
IEnumerable<IPAddress?> iPAddresses = responseObject.Nodes.Select(node=>node.IPAddress);
```

## Preforming an update request

```c#
GnutellaNode node = new GnutellaNode("127.0.0.1", 43624)
UpdateRequest updateRequest = new() { GnutellaNode = node, Network = GnutellaNetwork.Gnutella2 };
Result<UpdateResponse> updateResponse = clientV2.Update(updateRequest);
if(!updateResponse.WasSuccessful){
	throw new Exception(updateResponse.ErrorMessage);
}
```