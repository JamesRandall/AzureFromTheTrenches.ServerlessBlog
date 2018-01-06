using System.IO;
using System.Threading.Tasks;
using ServerlessBlog.DataAccess.Implementation.Parsing;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation
{
    internal class PostRepository : IPostRepository
    {
        private readonly IBlobStore _blobStore;
        private readonly IPostParser _postParser;
        private readonly string _defaultAuthor;

        public PostRepository(IBlobStoreFactory blobStoreFactory, IPostParser postParser, string defaultAuthor)
        {
            _blobStore = blobStoreFactory.Create("posts");
            _postParser = postParser;
            _defaultAuthor = defaultAuthor;
            
        }

        public async Task<Post> Get(string name)
        {
            string text = await _blobStore.Get(name);
            return _postParser.FromString(text, _defaultAuthor);                         
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
