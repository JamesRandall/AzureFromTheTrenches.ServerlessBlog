using System.IO;
using System.Threading.Tasks;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess
{
    public interface IPostRepository
    {
        Task<Post> Get(string name);

        Task<Post> Get(Stream stream);
    }
}
