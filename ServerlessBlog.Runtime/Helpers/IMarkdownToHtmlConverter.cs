using System;

namespace ServerlessBlog.Runtime.Helpers
{
    interface IMarkdownToHtmlConverter
    {
        string FromMarkdown(string markdown, string urlName, string author, DateTime postedAtUtc);
    }
}
