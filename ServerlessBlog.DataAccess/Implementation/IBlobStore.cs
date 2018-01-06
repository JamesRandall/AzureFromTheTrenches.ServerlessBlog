using System.Threading.Tasks;

namespace ServerlessBlog.DataAccess.Implementation
{
    internal interface IBlobStore
    {
        Task<string> Get(string name);
        Task Save(string filename, string text);
    }
}