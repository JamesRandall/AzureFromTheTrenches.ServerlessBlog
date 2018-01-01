using System;
using System.Text;

namespace ServerlessBlog.Runtime.Helpers.Implementation
{
    internal class MarkdownToHtmlConverter : IMarkdownToHtmlConverter
    {
        public string FromMarkdown(string markdown, string urlName, string author, DateTime postedAtUtc)
        {
            string html = CommonMark.CommonMarkConverter.Convert(markdown);

            // we have to munge in the author and post date here as it won't appear in the markdown source
            int endOfTitle = html.IndexOf("</h1>", StringComparison.InvariantCultureIgnoreCase);
            if (endOfTitle == -1)
            {
                return html;
            }
            endOfTitle += 5;

            string titleWithLink = html.Substring(0, endOfTitle).Replace("<h1>", $"<h1 class='post-title'><a href='/{urlName}'>").Replace("</h1>", "</a></h1>");
            string archiveUrl = $"/archive/{postedAtUtc.Year:D4}/{postedAtUtc.Month:D2}";
            string lineToInsert = $"<div class='post-metadata'>Posted on <a href='{archiveUrl}'>{postedAtUtc:D}</a> by {author}</div>";

            StringBuilder sb = new StringBuilder(titleWithLink);
            sb.AppendLine(lineToInsert);
            sb.AppendLine(html.Substring(endOfTitle+1));

            string result = sb.ToString();
            return result;
        }
    }
}
