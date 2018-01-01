using System.Linq;
using System.Threading.Tasks;
using ServerlessBlog.DataAccess;
using ServerlessBlog.Model;
using ServerlessBlog.Runtime.Helpers;

namespace ServerlessBlog.Runtime.StaticAssetManagement.Implementation
{
    class CategoryRenderer : ICategoryRenderer
    {
        private class TemplatePayload
        {
            public string DisplayName { get; set; }

            public string[] Posts { get; set; }
        }

        private readonly IOutputRepository _outputRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly ITemplateEngine _templateEngine;
        private readonly IPostSummaryGenerator _postSummaryGenerator;

        public CategoryRenderer(IOutputRepository outputRepository,
            ITemplateRepository templateRepository,
            ITemplateEngine templateEngine,
            IPostSummaryGenerator postSummaryGenerator)
        {
            _outputRepository = outputRepository;
            _templateRepository = templateRepository;
            _templateEngine = templateEngine;
            _postSummaryGenerator = postSummaryGenerator;
        }

        public async Task<string> Render(Category category)
        {
            PostSummary[] posts = category.Posts.ToArray();
            Task<string>[] getPostTasks = new Task<string>[posts.Length];
            for (int postIndex = 0; postIndex < category.Posts.Count; postIndex++)
            {
                getPostTasks[postIndex] = _outputRepository.GetPost(posts[postIndex].UrlName);
            }
            await Task.WhenAll(getPostTasks);

            string[] textForSummaries = getPostTasks.Select(x => x.Result).ToArray();
            string[] titles = posts.Select(x => x.Title).ToArray();
            string[] summaries = _postSummaryGenerator.ExtractSummaries(titles, textForSummaries);

            string template = await _templateRepository.GetCategoryTemplate();
            string html = _templateEngine.Execute(template, new TemplatePayload
            {
                DisplayName = category.DisplayName,
                Posts = summaries
            });

            return html;
        }        
    }
}
