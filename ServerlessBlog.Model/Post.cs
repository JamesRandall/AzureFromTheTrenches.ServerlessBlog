using System;

namespace ServerlessBlog.Model
{
    public class Post
    {
        public Post()
        {
            Categories = new Category[0];
        }
        
        public string Title { get; set; }

        public string UrlName { get; set; }

        public DateTime PostedAtUtc { get; set; }

        public string Author { get; set; }

        public Category[] Categories { get; set; }

        public string Markdown { get; set; }
    }
}
