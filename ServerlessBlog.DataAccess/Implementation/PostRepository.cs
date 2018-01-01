using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using ServerlessBlog.DataAccess.Implementation.Parsing;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation
{
    internal class PostRepository : IPostRepository
    {
        private readonly IPostParser _postParser;
        private readonly string _defaultAuthor;
        private readonly CloudBlobContainer _blobContainer;

        public PostRepository(string storageConnectionString, IPostParser postParser, string defaultAuthor)
        {
            _postParser = postParser;
            _defaultAuthor = defaultAuthor;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = blobClient.GetContainerReference("posts");
        }

        public async Task<Post> Get(string name)
        {
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(name);
            using (Stream stream = await blob.OpenReadAsync())
            {
                return await Get(stream);
            }                            
        }

        public Task<Post> Get(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string postText = reader.ReadToEnd();
            Post post = _postParser.FromString(postText, _defaultAuthor);
            return Task.FromResult(post);
        }

    }
}
