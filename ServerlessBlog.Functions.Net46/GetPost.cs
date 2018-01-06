using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using ServerlessBlog.Model;
using ServerlessBlog.Runtime;

namespace ServerlessBlog.Functions.Net46
{
    public static class GetPost
    {
        [FunctionName("GetPost")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "post/{postName}")]HttpRequestMessage req, string postName, TraceWriter log)
        {
            log.Info($"GetPost triggered for {postName}");
            Factory.Create(ConfigurationOptionsFactory.Create(), CloudVendorEnum.Azure);

            IWebPageComposer webPageComposer = Factory.Instance.GetResponseRenderer();
            string content = await webPageComposer.GetPost(postName);

            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(content, Encoding.UTF8, "text/html");

            return response;
        }
    }
}
