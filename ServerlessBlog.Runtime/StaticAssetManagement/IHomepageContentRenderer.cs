using System.Threading.Tasks;

namespace ServerlessBlog.Runtime.StaticAssetManagement
{
    internal interface IHomepageContentRenderer
    {
        Task<string> Render(string[] urlNames);
    }
}