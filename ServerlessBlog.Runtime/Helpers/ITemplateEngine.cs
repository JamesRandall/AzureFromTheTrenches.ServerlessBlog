namespace ServerlessBlog.Runtime.Helpers
{
    internal interface ITemplateEngine
    {
        string Execute(string template, object payload);
    }
}
