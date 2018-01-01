using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace ServerlessBlog.Functions.Net46
{
    public static class Tickle
    {
        [FunctionName("Tickle")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
            string domain = ConfigurationOptionsFactory.Create().BlogDomain;
            using (HttpClient client = new HttpClient())
            {
                Uri uri = new Uri($"http://{domain}/");
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    log.Info(result);
                }
                else
                {
                    log.Warning($"Call to {uri} failed with status code {response.StatusCode}");
                }
            }
        }
    }
}
