using System;

namespace ServerlessBlog.DataAccess.Implementation.Model
{
    // all settings are optional
    internal class PostOptions
    {
        public DateTime? CreatedAtUtc { get; set; }

        public string[] Categories { get; set; }

        public string UrlName { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }
    }
}
