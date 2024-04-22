using GWebCache.Reponses;

namespace GWebCache.ReponseProcessing;

internal interface IParseable<T> where T : GWebCacheResponse {
	abstract void Parse(HttpResponseMessage response);
	abstract void ParseV2(HttpResponseMessage response);
}
