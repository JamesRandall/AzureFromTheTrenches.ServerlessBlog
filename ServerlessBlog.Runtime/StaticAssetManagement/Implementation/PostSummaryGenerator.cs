using System;

namespace ServerlessBlog.Runtime.StaticAssetManagement.Implementation
{
    internal class PostSummaryGenerator : IPostSummaryGenerator
    {
        public string[] ExtractSummaries(string[] titles, string[] textForSummaries)
        {
            // this is a cheap and dirty way of getting out summaries. based on my markdown I always have a title and a first paragraph
            // and this attempts to find that without much sanity checking
            string[] summaries = new string[textForSummaries.Length];
            for (int postIndex = 0; postIndex < textForSummaries.Length; postIndex++)
            {
                int closingParagraphTag = textForSummaries[postIndex].IndexOf("</p>", StringComparison.Ordinal);
                if (closingParagraphTag == -1)
                {
                    summaries[postIndex] = $"<h1>{titles[postIndex]}</h1>";
                }
                summaries[postIndex] = textForSummaries[postIndex].Substring(0, closingParagraphTag + 4);
            }
            return summaries;
        }
    }
}
