using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using ServerlessBlog.Runtime;

namespace ServerlessBlog.Functions.Net46
{
    public static class GetArchive
    {
        [FunctionName("GetArchive")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "archive/{year}/{month}")]HttpRequestMessage req, int year, int month, TraceWriter log)
        {
            log.Info($"GetArchive triggered for {year}-{month}");
            Factory.Create(ConfigurationOptionsFactory.Create());

            IWebPageComposer webPageComposer = Factory.Instance.GetResponseRenderer();
            string content = await webPageComposer.GetArchive(year, month);

            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(content, Encoding.UTF8, "text/html");

            return response;
        }
    }
}
