using System.Collections.Generic;
using System.Threading.Tasks;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess
{
    public interface IPostingTimeRepository
    {
        Task<IReadOnlyCollection<PostingTime>> Get();

        Task InsertOrUpdate(Post post);
    }
}
