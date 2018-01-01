using System.Threading.Tasks;

namespace ServerlessBlog.DataAccess
{
    public interface ITemplateRepository
    {
        Task<string> GetLayoutTemplate();

        Task<string> GetSidebarTemplate();

        Task<string> GetHomepageTemplate();

        Task<string> GetArchiveTemplate();

        Task<string> GetCategoryTemplate();
    }
}
