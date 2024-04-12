using GWebCache.ReponseProcessing;

namespace GWebCache.Reponses
{
    public abstract class GWebCacheResponse : IParseable<GWebCacheResponse>, IValidator
    {
        public virtual bool IsValidResponse(HttpResponseMessage? responseMessage)
        {
            return responseMessage != null && responseMessage.IsSuccessStatusCode && responseMessage.Content != null;
        }

        public abstract void Parse(HttpResponseMessage? response);
    }
}
