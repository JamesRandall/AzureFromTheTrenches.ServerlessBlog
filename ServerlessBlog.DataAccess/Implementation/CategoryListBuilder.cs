using System.Collections.Generic;
using System.Linq;
using ServerlessBlog.DataAccess.Implementation.Model;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation
{
    internal class CategoryListBuilder : ICategoryListBuilder
    {
        public IReadOnlyCollection<Category> FromCategoryItems(IEnumerable<CategoryItem> items)
        {
            var result = items.GroupBy(x => x.UrlName, (k, g) => new Category
            {
                UrlName = k,
                DisplayName = g.First().DisplayName,
                Posts = g.OrderByDescending(x => x.PostedAtUtc).Select(x => new PostSummary
                {
                    PostedAtUtc = x.PostedAtUtc,
                    Title = x.PostTitle,
                    UrlName = x.PostUrlName
                }).ToArray()
            }).OrderBy(x => x.DisplayName).ToArray();

            return result;
        }
    }
}
