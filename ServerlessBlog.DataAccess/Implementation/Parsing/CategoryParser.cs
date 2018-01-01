using ServerlessBlog.DataAccess.Implementation.Extensions;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation.Parsing
{
    internal class CategoryParser : ICategoryParser
    {
        public Category FromString(string categoryString)
        {
            return new Category
            {
                DisplayName = categoryString,
                UrlName = categoryString.ToUrlString()
            };
        }
    }
}
