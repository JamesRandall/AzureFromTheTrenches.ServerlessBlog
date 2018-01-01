using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServerlessBlog.DataAccess;
using ServerlessBlog.Model;
using ServerlessBlog.Runtime.Helpers;

namespace ServerlessBlog.Runtime.StaticAssetManagement.Implementation
{
    internal class ArchiveRenderer : IArchiveRenderer
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

        public ArchiveRenderer(IOutputRepository outputRepository,
            ITemplateRepository templateRepository,
            ITemplateEngine templateEngine,
            IPostSummaryGenerator postSummaryGenerator)
        {
            _outputRepository = outputRepository;
            _templateRepository = templateRepository;
            _templateEngine = templateEngine;
            _postSummaryGenerator = postSummaryGenerator;
        }

        public async Task<string> Render(DateTime forDate, IReadOnlyCollection<PostingTime> postingTimes)
        {
            PostingTime[] includedPosts = postingTimes
                .Where(x => x.PostedAtUtc.Year == forDate.Year && x.PostedAtUtc.Month == forDate.Month).ToArray();
            string archiveTitle = forDate.ToString(Constants.ArchiveStringFormat);

            Task<string>[] getPostTasks = new Task<string>[includedPosts.Length];
            for (int postIndex = 0; postIndex < includedPosts.Length; postIndex++)
            {
                getPostTasks[postIndex] = _outputRepository.GetPost(includedPosts[postIndex].UrlName);
            }
            await Task.WhenAll(getPostTasks);

            
            string[] textForSummaries = getPostTasks.Select(x => x.Result).ToArray();
            string[] titles = postingTimes.Select(x => x.Title).ToArray();
            string[] summaries = _postSummaryGenerator.ExtractSummaries(titles, textForSummaries);

            string template = await _templateRepository.GetArchiveTemplate();
            string html = _templateEngine.Execute(template, new TemplatePayload
            {
                DisplayName = archiveTitle,
                Posts = summaries
            });

            return html;
        }
    }
}
