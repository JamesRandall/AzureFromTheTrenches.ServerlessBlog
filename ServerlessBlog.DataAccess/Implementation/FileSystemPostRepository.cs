using System.IO;
using System.Threading.Tasks;
using ServerlessBlog.DataAccess.Implementation.Parsing;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation
{
    internal class FileSystemPostRepository : IPostRepository
    {
        private readonly IPostParser _postParser;
        private readonly string _defaultAuthor;

        public FileSystemPostRepository(IPostParser postParser, string defaultAuthor)
        {
            _postParser = postParser;
            _defaultAuthor = defaultAuthor;
        }

        public Task<Post> Get(string name)
        {
            string postText = System.IO.File.ReadAllText(name);
            Post post = _postParser.FromString(postText, _defaultAuthor);
            return Task.FromResult(post);
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
