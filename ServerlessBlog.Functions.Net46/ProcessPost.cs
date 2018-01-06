using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using ServerlessBlog.Model;
using ServerlessBlog.Runtime;

namespace ServerlessBlog.Functions.Net46
{
    public static class ProcessPost
    {
        [FunctionName("ProcessPost")]
        public static async Task Run([BlobTrigger("posts/{name}", Connection = "BlogStorage")]Stream myBlob, string name, TraceWriter log)
        {
            log.Info($"ProcessPost triggered\n Blob Name:{name} \n Size: {myBlob.Length} Bytes");

            Factory.Create(ConfigurationOptionsFactory.Create(), CloudVendorEnum.Azure);

            IStaticAssetManager staticAssetManager = Factory.Instance.GetRenderer();
            await staticAssetManager.AddOrUpdatePost(myBlob);
        }
    }
}
