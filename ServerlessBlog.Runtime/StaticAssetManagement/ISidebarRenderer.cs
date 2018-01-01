using System.Collections.Generic;
using System.Threading.Tasks;
using ServerlessBlog.Model;

namespace ServerlessBlog.Runtime.StaticAssetManagement
{
    internal interface ISidebarRenderer
    {
        Task<string> Render(IReadOnlyCollection<PostingTime> reentPosts, IReadOnlyCollection<Archive> archives, IReadOnlyCollection<Category> categories);
    }
}
