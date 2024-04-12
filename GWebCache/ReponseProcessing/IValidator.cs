namespace GWebCache.ReponseProcessing
{
    internal interface IValidator
    {
        bool IsValidResponse(HttpResponseMessage responseMessage);
    }
}
