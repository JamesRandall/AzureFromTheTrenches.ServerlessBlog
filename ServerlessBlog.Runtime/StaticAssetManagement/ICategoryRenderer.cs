using System.Threading.Tasks;
using ServerlessBlog.Model;

namespace ServerlessBlog.Runtime.StaticAssetManagement
{
    internal interface ICategoryRenderer
    {
        Task<string> Render(Category category);
    }
}
