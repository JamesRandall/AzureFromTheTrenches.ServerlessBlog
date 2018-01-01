using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HandlebarsDotNet;
using ServerlessBlog.DataAccess;
using ServerlessBlog.Model;

namespace ServerlessBlog.Runtime.StaticAssetManagement.Implementation
{
    class SidebarRenderer : ISidebarRenderer
    {
        private class TemplatePayload
        {
            public IReadOnlyCollection<PostingTime> RecentPosts { get; set; }

            public IReadOnlyCollection<Archive> Archives { get; set; }

            public IReadOnlyCollection<Category> Categories { get; set; }
        }

        private readonly ITemplateRepository _templateRepository;

        public SidebarRenderer(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task<string> Render(IReadOnlyCollection<PostingTime> recentPosts, IReadOnlyCollection<Archive> archives, IReadOnlyCollection<Category> categories)
        {
            string template = await _templateRepository.GetSidebarTemplate();
            Func<object,string> compiledTemplate = Handlebars.Compile(template);
            string result = compiledTemplate(new TemplatePayload
            {
                Archives = archives,
                Categories = categories,
                RecentPosts = recentPosts
            });
            return result;
        }
    }
}
