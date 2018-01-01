using System.IO;
using System.Threading.Tasks;

namespace ServerlessBlog.Runtime
{
    public interface IStaticAssetManager
    {
        Task AddOrUpdatePost(Stream postStream);
    }
}
