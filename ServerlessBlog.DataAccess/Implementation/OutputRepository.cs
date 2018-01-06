using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ServerlessBlog.DataAccess.Implementation
{
    class OutputRepository : IOutputRepository
    {
        private readonly IBlobStore _blobStore;
        private const string SidebarSnippetName = "sidebar";
        private const string HomepageSnippetName = "homepage";

        public OutputRepository(IBlobStoreFactory blobStoreFactory)
        {
            _blobStore = blobStoreFactory.Create("output");
        }

        public Task<string> GetCategory(string categoryUrlName)
        {
            return GetSnippet($"__{categoryUrlName}");
        }

        public async Task SavePost(string urlName, string htmlSnippet)
        {
            if (urlName.ToLower() == SidebarSnippetName || urlName.ToLower() == HomepageSnippetName || IsArchive(urlName) || urlName.StartsWith("__"))
            {
                throw new ReservedNameException($"The post name {urlName} is reserved for system use.");
            }
            await SaveSnippet(urlName, htmlSnippet);
        }

        private bool IsArchive(string urlName)
        {
            if (urlName.Length != 6)
                return false;

            foreach (char c in urlName)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public async Task SaveSidebar(string htmlSnippet)
        {
            await SaveSnippet(SidebarSnippetName, htmlSnippet);
        }

        public async Task SaveHomepageContent(string homepageSnippet)
        {
            await SaveSnippet(HomepageSnippetName, homepageSnippet);
        }

        public async Task SaveArchive(int year, int month, string htmlSnippet)
        {
            await SaveSnippet($"{year:D4}{month:D2}", htmlSnippet);
        }

        public async Task SaveCategory(string categoryUrlName, string htmlSnippet)
        {
            await SaveSnippet($"__{categoryUrlName}", htmlSnippet);
        }

        public async Task<string> GetHomepageContent()
        {
            return await GetSnippet(HomepageSnippetName);
        }

        public async Task<string> GetPost(string urlName)
        {
            return await GetSnippet(urlName);
        }

        public async Task<string> GetSidebar()
        {
            return await GetSnippet(SidebarSnippetName);
        }

        public Task<string> GetArchive(int year, int month)
        {
            return GetSnippet($"{year:D4}{month:D2}");
        }

        private async Task<string> GetSnippet(string name)
        {
            string filename = GetAssetFilename(name);
            string result = await _blobStore.Get(filename);
            return result;
        }


        private string GetAssetFilename(string urlName)
        {
            return $"{urlName}.html.snippet";
        }

        private async Task SaveSnippet(string name, string htmlSnippet)
        {
            string filename = GetAssetFilename(name);
            await _blobStore.Save(filename, htmlSnippet);
        }
    }
}
