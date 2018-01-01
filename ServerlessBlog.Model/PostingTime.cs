using System;

namespace ServerlessBlog.Model
{
    public class PostingTime
    {
        public string UrlName { get; set; }

        public DateTime PostedAtUtc { get; set; }

        public string Title { get; set; }
    }
}
