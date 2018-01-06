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
    public static class GetCategory
    {
        [FunctionName("GetCategory")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "category/{category}")]HttpRequestMessage req, string category, TraceWriter log)
        {
            log.Info($"GetCategory triggered for {category}");
            Factory.Create(ConfigurationOptionsFactory.Create(), CloudVendorEnum.Azure);

            IWebPageComposer webPageComposer = Factory.Instance.GetResponseRenderer();
            string content = await webPageComposer.GetCategory(category);

            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(content, Encoding.UTF8, "text/html");

            return response;
        }
    }
}
