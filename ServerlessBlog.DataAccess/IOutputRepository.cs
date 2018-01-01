using System;
using System.Threading.Tasks;

namespace ServerlessBlog.DataAccess
{
    public interface IOutputRepository
    {
        Task<string> GetSidebar();
        Task<string> GetPost(string urlName);
        Task<string> GetHomepageContent();
        Task<string> GetArchive(int year, int month);
        Task<string> GetCategory(string categoryUrlName);
        Task SavePost(string urlName, string htmlSnippet);
        Task SaveSidebar(string htmlSnippet);
        Task SaveHomepageContent(string homepageSnippet);
        Task SaveArchive(int year, int month, string htmlSnippet);
        Task SaveCategory(string categoryUrlName, string htmlSnippet);
    }
}
