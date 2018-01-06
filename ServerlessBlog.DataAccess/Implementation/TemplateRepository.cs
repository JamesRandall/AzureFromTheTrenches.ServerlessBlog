using System.Threading.Tasks;

namespace ServerlessBlog.DataAccess.Implementation
{
    internal class TemplateRepository : ITemplateRepository
    {
        private readonly IBlobStore _blobStore;

        public TemplateRepository(IBlobStoreFactory blobStoreFactory)
        {
            _blobStore = blobStoreFactory.Create("templates");
        }

        public Task<string> GetLayoutTemplate()
        {
            return GetWithFilename("layout.handlebars");
        }

        public Task<string> GetSidebarTemplate()
        {
            return GetWithFilename("sidebar.handlebars");
        }

        public Task<string> GetHomepageTemplate()
        {
            return GetWithFilename("homepage.handlebars");
        }

        public Task<string> GetArchiveTemplate()
        {
            return GetWithFilename("archive.handlebars");
        }

        public Task<string> GetCategoryTemplate()
        {
            return GetWithFilename("category.handlebars");
        }

        private async Task<string> GetWithFilename(string filename)
        {
            string result = await _blobStore.Get(filename);
            return result;
        }
    }
}
