using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ServerlessBlog.DataAccess.Implementation.Extensions;
using ServerlessBlog.DataAccess.Implementation.Model;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation.Parsing
{
    internal class PostParser : IPostParser
    {
        private readonly ICategoryParser _categoryParser;

        public PostParser(ICategoryParser categoryParser)
        {
            _categoryParser = categoryParser;
        }

        public Post FromString(string post, string defaultAuthor)
        {
            // Posts are in two parts - json and then markdown
            // we do a really basic split in two based on the markdown always starting with a title
            // in the format:
            //  # This is a title
            // and treat everything above as json and everything this point on as markdown
            // We could do this by looking for the # more cleverly (first character on first line or follows a new line)
            // but I think we're going to revisit this to do additional parsing (code blocks)
            StringBuilder jsonBuilder = new StringBuilder();
            StringBuilder markdownBuilder = new StringBuilder();
            StringBuilder currentBuilder = jsonBuilder;
            string markdownTitle = null;
            string[] lines = Regex.Split(post, "\r\n|\r|\n");;
            foreach (string line in lines)
            {
                if (currentBuilder == jsonBuilder)
                {
                    if (line.StartsWith("#"))
                    {
                        currentBuilder = markdownBuilder;
                        markdownTitle = line.Substring(1).Trim();
                    }
                }
                currentBuilder.AppendLine(line);
            }

            string json = jsonBuilder.ToString();
            if (string.IsNullOrWhiteSpace(json))
            {
                json = "{ }";
            }
            PostOptions options = JsonConvert.DeserializeObject<PostOptions>(json);
            Post result = new Post
            {
                Author = string.IsNullOrWhiteSpace(options.Author) ? defaultAuthor : options.Author,
                Categories = options.Categories?.Select(x => _categoryParser.FromString(x)).ToArray() ?? new[] { _categoryParser.FromString("Uncategorised")},
                Markdown = markdownBuilder.ToString(),
                PostedAtUtc = options.CreatedAtUtc ?? DateTime.UtcNow,
                Title = string.IsNullOrWhiteSpace(options.Title) ? markdownTitle : options.Title,
                UrlName = string.IsNullOrWhiteSpace(options.UrlName) ? markdownTitle.ToUrlString() : options.UrlName
            };

            return result;
        }
    }
}
