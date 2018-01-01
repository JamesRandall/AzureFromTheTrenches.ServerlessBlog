using System;
using System.Collections.Generic;
using System.Text;

namespace ServerlessBlog.Model
{
    public class PostSummary
    {
        public string Title { get; set; }

        public DateTime PostedAtUtc { get; set; }

        public string UrlName { get; set; }
    }
}
