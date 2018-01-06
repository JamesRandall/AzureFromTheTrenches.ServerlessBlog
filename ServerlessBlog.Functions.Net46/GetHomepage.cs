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
    public static class GetHomepage
    {
        [FunctionName("GetHomepage")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "home")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("GetHomepage triggered");
            Factory.Create(ConfigurationOptionsFactory.Create(), CloudVendorEnum.Azure);

            IWebPageComposer webPageComposer = Factory.Instance.GetResponseRenderer();
            string content = await webPageComposer.GetHomepage();

            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(content, Encoding.UTF8, "text/html");            

            return response;            
        }
    }
}
