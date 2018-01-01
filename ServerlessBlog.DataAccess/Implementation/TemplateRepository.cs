using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ServerlessBlog.DataAccess.Implementation
{
    internal class TemplateRepository : ITemplateRepository
    {
        private readonly CloudBlobContainer _blobContainer;

        public TemplateRepository(string storageConnectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = blobClient.GetContainerReference("templates");
        }

        public Task<string> GetLayoutTemplate()
        {
            return GetWithFilename("layout.handlebars");
        }

        public Task<string> GetSidebarTemplate()
        {
            return GetWithFilename("sidebar.handlebars");
        }

        public Task<string> GetHomepageTemplate()
        {
            return GetWithFilename("homepage.handlebars");
        }

        public Task<string> GetArchiveTemplate()
        {
            return GetWithFilename("archive.handlebars");
        }

        public Task<string> GetCategoryTemplate()
        {
            return GetWithFilename("category.handlebars");
        }

        private async Task<string> GetWithFilename(string filename)
        {
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(filename);
            string result = await blob.DownloadTextAsync();
            return result;
        }
    }
}
