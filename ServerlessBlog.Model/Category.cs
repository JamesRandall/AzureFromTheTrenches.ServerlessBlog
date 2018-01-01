using System.Collections.Generic;

namespace ServerlessBlog.Model
{
    public class Category
    {
        public string DisplayName { get; set; }

        public string UrlName { get; set; }

        public IReadOnlyCollection<PostSummary> Posts { get; set; } 
    }
}
