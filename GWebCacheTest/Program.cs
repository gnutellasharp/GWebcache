using GWebCache;

namespace GWebCacheTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
           IGWebCacheClient client = new GWebCacheClient("http://gweb3.4octets.co.uk/gwc.php");
            var stats = client.GetStats();
           if(client.CheckIfAlive()  && stats.WasSuccessful && stats.ResultObject != null)
            {
               System.Console.WriteLine($"Total number of requests: {stats.ResultObject.TotalNumberOfRequests}");
               System.Console.WriteLine($"Requests in last hour: {stats.ResultObject.RequestsInLastHour}");
               System.Console.WriteLine($"Update requests in last hour: {stats.ResultObject.UpdateRequestsInLastHour}");
           }
        }
    }
}
