using System.Collections.Generic;
using ServerlessBlog.DataAccess.Implementation.Model;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation
{
    internal interface ICategoryListBuilder
    {
        IReadOnlyCollection<Category> FromCategoryItems(IEnumerable<CategoryItem> items);
    }
}