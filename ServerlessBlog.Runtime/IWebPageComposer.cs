using System.Threading.Tasks;

namespace ServerlessBlog.Runtime
{
    public interface IWebPageComposer
    {
        Task<string> GetHomepage();

        Task<string> GetPost(string urlName);

        Task<string> GetArchive(int year, int month);

        Task<string> GetCategory(string category);
    }
}
