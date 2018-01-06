using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using ServerlessBlog.Runtime;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ServerlessBlog.Functions.AWSLambda
{
    public class Functions
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public async Task<APIGatewayProxyResponse> GetHomepage(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("GetHomepage triggered");
            Factory.Create(ConfigurationOptionsFactory.Create());

            IWebPageComposer webPageComposer = Factory.Instance.GetResponseRenderer();
            string content = await webPageComposer.GetHomepage();

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = content,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/html" } }
            };

            return response;
        }
    }
}
