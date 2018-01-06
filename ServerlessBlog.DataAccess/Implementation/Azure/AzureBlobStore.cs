using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ServerlessBlog.DataAccess.Implementation.Azure
{
    class AzureBlobStore : IBlobStore
    {
        private readonly CloudBlobContainer _blobContainer;

        public AzureBlobStore(string storageConnectionString, string folder)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = blobClient.GetContainerReference(folder);
        }

        public async Task<string> Get(string name)
        {
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(name);
            using (Stream stream = await blob.OpenReadAsync())
            {
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();
                return text;
            }
        }

        public async Task Save(string filename, string text)
        {
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(filename);
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
        }
    }
}
