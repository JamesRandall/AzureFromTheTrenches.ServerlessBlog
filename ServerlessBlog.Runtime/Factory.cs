using ServerlessBlog.Model;
using ServerlessBlog.Runtime.Composition;
using ServerlessBlog.Runtime.Helpers;
using ServerlessBlog.Runtime.Helpers.Implementation;
using ServerlessBlog.Runtime.StaticAssetManagement;
using ServerlessBlog.Runtime.StaticAssetManagement.Implementation;

namespace ServerlessBlog.Runtime
{
    public class Factory
    {
        private readonly IConfigurationOptions _configuration;
        public static Factory Instance;

        private Factory(IConfigurationOptions configuration, CloudVendorEnum cloudVendor)
        {
            _configuration = configuration;
            DataAccess.Factory.Create(configuration, cloudVendor);
        }

        public static void Create(IConfigurationOptions configuration, CloudVendorEnum cloudVendor)
        {
            Instance = new Factory(configuration, cloudVendor);
        }

        public IStaticAssetManager GetRenderer() => new StaticAssetManager(
            GetArchiveRenderer(),
            GetSidebarRenderer(),
            GetCategoryRenderer(),
            GetHomepageRenderer(),            
            new MarkdownToHtmlConverter(),
            new ArchiveListBuilder(),
            DataAccess.Factory.Instance.GetPostRepository(),
            DataAccess.Factory.Instance.GetPostingTimeRepository(),
            DataAccess.Factory.Instance.GetOutputRepository(),
            DataAccess.Factory.Instance.GetCategoryRepository());

        internal ISidebarRenderer GetSidebarRenderer() => new SidebarRenderer(DataAccess.Factory.Instance.GetTemplateRepository());

        internal IHomepageContentRenderer GetHomepageRenderer() => new HomepageContentRenderer(
            DataAccess.Factory.Instance.GetOutputRepository(),
            DataAccess.Factory.Instance.GetTemplateRepository());

        internal IArchiveRenderer GetArchiveRenderer() => new ArchiveRenderer(
            DataAccess.Factory.Instance.GetOutputRepository(),
            DataAccess.Factory.Instance.GetTemplateRepository(),
            GetTemplateEngine(),
            GetPostSummaryGenerator());

        internal ICategoryRenderer GetCategoryRenderer() => new CategoryRenderer(
            DataAccess.Factory.Instance.GetOutputRepository(),
            DataAccess.Factory.Instance.GetTemplateRepository(),
            GetTemplateEngine(),
            GetPostSummaryGenerator());

        public IWebPageComposer GetResponseRenderer() => new WebPageComposer(_configuration.BlogName,
            _configuration.DefaultAuthor,
            _configuration.StylesheetUrl,
            _configuration.FavIconUrl,
            DataAccess.Factory.Instance.GetTemplateRepository(),
            DataAccess.Factory.Instance.GetOutputRepository());

        internal  ITemplateEngine GetTemplateEngine() => new TemplateEngine();

        internal IPostSummaryGenerator GetPostSummaryGenerator() => new PostSummaryGenerator();
    }
}
