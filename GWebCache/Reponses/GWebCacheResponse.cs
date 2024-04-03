namespace GWebCache.Reponses
{
    public class GWebCacheResponse
    {
        public bool WasSuccessful { get; set; }
        protected static GWebCacheResponse Parse(HttpResponseMessage response) {
            GWebCacheResponse result = new()
            {
                WasSuccessful = response.IsSuccessStatusCode && response.Content != null
            };
            return result;
        }
    }
}
