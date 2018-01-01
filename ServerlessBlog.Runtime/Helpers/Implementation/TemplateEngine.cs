using System;
using HandlebarsDotNet;

namespace ServerlessBlog.Runtime.Helpers.Implementation
{
    class TemplateEngine : ITemplateEngine
    {
        public string Execute(string template, object payload)
        {
            Func<object,string> compiledTemplate = Handlebars.Compile(template);
            return compiledTemplate(payload);
        }
    }
}
