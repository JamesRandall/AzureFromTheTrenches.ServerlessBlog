using System;
using System.Threading.Tasks;
using HandlebarsDotNet;
using ServerlessBlog.DataAccess;

namespace ServerlessBlog.Runtime.Composition
{
    internal class WebPageComposer : IWebPageComposer
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IOutputRepository _outputRepository;

        private class TemplatePayload
        {
            public string PageTitle { get; set; }

            public string DefaultAuthor { get; set; }

            public string StylesheetUrl { get; set; }

            public string BlogName { get; set; }

            public string ReadingContent { get; set; }

            public string Sidebar { get; set; }

            public string FavIconUrl { get; set; }
        }

        private readonly string _defaultAuthor;
        private readonly string _stylesheetUrl;
        private readonly string _blogName;
        private readonly string _favIconUrl;

        public WebPageComposer(string blogName,
            string defaultAuthor,
            string stylesheetUrl,
            string favIconUrl,
            ITemplateRepository templateRepository,
            IOutputRepository outputRepository)
        {
            _templateRepository = templateRepository;
            _outputRepository = outputRepository;
            _defaultAuthor = defaultAuthor;
            _stylesheetUrl = stylesheetUrl;
            _blogName = blogName;
            _favIconUrl = favIconUrl;
        }

        public async Task<string> GetHomepage()
        {
            return await GetWrappedContent(() => _outputRepository.GetHomepageContent());
        }

        public async Task<string> GetPost(string urlName)
        {
            return await GetWrappedContent(() => _outputRepository.GetPost(urlName));
        }

        public async Task<string> GetArchive(int year, int month)
        {
            return await GetWrappedContent(() => _outputRepository.GetArchive(year, month));
        }

        public async Task<string> GetCategory(string category)
        {
            return await GetWrappedContent(() => _outputRepository.GetCategory(category));
        }

        private async Task<string> GetWrappedContent(Func<Task<string>> contentFunc)
        {
            Task<string> templateTask = _templateRepository.GetLayoutTemplate();
            Task<string> sidebarTask = _outputRepository.GetSidebar();
            Task<string> contentTask = contentFunc();

            await Task.WhenAll(templateTask, sidebarTask, contentTask);

            string template = templateTask.Result;
            string content = contentTask.Result;
            string sidebar = sidebarTask.Result;

            TemplatePayload payload = new TemplatePayload
            {
                BlogName = _blogName,
                DefaultAuthor = _defaultAuthor,
                PageTitle = _blogName,
                ReadingContent = content,
                Sidebar = sidebar,
                StylesheetUrl = _stylesheetUrl,
                FavIconUrl = _favIconUrl
            };
            Func<object, string> compiledTemplate = Handlebars.Compile(template);

            string html = compiledTemplate(payload);
            return html;
        }
    }
}
