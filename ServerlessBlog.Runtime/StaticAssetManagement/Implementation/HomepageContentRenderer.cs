using System;
using System.Threading.Tasks;
using HandlebarsDotNet;
using ServerlessBlog.DataAccess;

namespace ServerlessBlog.Runtime.StaticAssetManagement.Implementation
{
    internal class HomepageContentRenderer : IHomepageContentRenderer
    {
        private readonly IOutputRepository _outputRepository;
        private readonly ITemplateRepository _templateRepository;

        public HomepageContentRenderer(IOutputRepository outputRepository,
            ITemplateRepository templateRepository)
        {
            _outputRepository = outputRepository;
            _templateRepository = templateRepository;
        }

        public async Task<string> Render(string[] urlNames)
        {
            string[] content = new string[urlNames.Length];
            for (int index = 0; index < urlNames.Length; index++)
            {
                content[index] = await _outputRepository.GetPost(urlNames[index]);
            }

            string template = await _templateRepository.GetHomepageTemplate();
            Func<object,string> compiledTemplate = Handlebars.Compile(template);
            string result = compiledTemplate(content);
            return result;
        }
    }
}
