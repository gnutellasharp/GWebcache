namespace GWebCache.Reponses
{
    internal class PongResponse : GWebCacheResponse, IParseable<PongResponse>
    {
        public string Message { get; set; }


        public static async Task<PongResponse> ParseAsync(HttpResponseMessage? response)
        {

            var result = new PongResponse();
            var content = await response?.Content.ReadAsStringAsync() ?? "";

            result.WasSuccessful = GWebCacheResponse.Parse(response).WasSuccessful &&
                (content.Contains("pong", StringComparison.InvariantCultureIgnoreCase));
            result.Message = content;

            return result;
        }
    }
}
