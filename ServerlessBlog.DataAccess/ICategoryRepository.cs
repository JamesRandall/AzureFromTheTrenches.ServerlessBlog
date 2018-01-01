using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess
{
    public interface ICategoryRepository
    {
        Task<IReadOnlyCollection<Category>> Get();

        Task<IReadOnlyCollection<Category>> Get(params string[] categoryUrlNames);

        Task InsertOrUpdate(Post post);
    }
}
