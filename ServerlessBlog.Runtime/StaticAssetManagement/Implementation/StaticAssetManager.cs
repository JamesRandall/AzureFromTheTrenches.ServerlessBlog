using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ServerlessBlog.DataAccess;
using ServerlessBlog.Model;
using ServerlessBlog.Runtime.Helpers;

namespace ServerlessBlog.Runtime.StaticAssetManagement.Implementation
{
    class StaticAssetManager : IStaticAssetManager
    {
        private const int RecentPostsToShow = 5;
        private const int HomepagePostsToShow = 5;
        private readonly IArchiveRenderer _archiveRenderer;
        private readonly ISidebarRenderer _sidebarRenderer;
        private readonly ICategoryRenderer _categoryRenderer;
        private readonly IHomepageContentRenderer _homepageContentRenderer;
        private readonly IMarkdownToHtmlConverter _markdownToHtmlConverter;
        private readonly IArchiveListBuilder _archiveListBuilder;
        private readonly IPostRepository _postRepository;
        private readonly IPostingTimeRepository _postingTimeRepository;
        private readonly IOutputRepository _outputRepository;
        private readonly ICategoryRepository _categoryRepository;

        public StaticAssetManager(
            IArchiveRenderer archiveRenderer,
            ISidebarRenderer sidebarRenderer,
            ICategoryRenderer categoryRenderer,
            IHomepageContentRenderer homepageContentRenderer,
            IMarkdownToHtmlConverter markdownToHtmlConverter,
            IArchiveListBuilder archiveListBuilder,
            IPostRepository postRepository,            
            IPostingTimeRepository postingTimeRepository,
            IOutputRepository outputRepository,
            ICategoryRepository categoryRepository)
        {
            _archiveRenderer = archiveRenderer;
            _sidebarRenderer = sidebarRenderer;
            _categoryRenderer = categoryRenderer;
            _homepageContentRenderer = homepageContentRenderer;
            _markdownToHtmlConverter = markdownToHtmlConverter;
            _archiveListBuilder = archiveListBuilder;
            _postRepository = postRepository;
            _postingTimeRepository = postingTimeRepository;
            _outputRepository = outputRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task AddOrUpdatePost(Stream postStream)
        {
            // TODO: This needs to deal with a post changing post date (being in one month and then in another)
            // and also being removed from a category.

            Post post = await _postRepository.Get(postStream);
            string html = _markdownToHtmlConverter.FromMarkdown(post.Markdown, post.UrlName, post.Author, post.PostedAtUtc);
            await _outputRepository.SavePost(post.UrlName, html);

            await _postingTimeRepository.InsertOrUpdate(post);
            IReadOnlyCollection<PostingTime> postingTimes = await _postingTimeRepository.Get();

            await _categoryRepository.InsertOrUpdate(post);
            IReadOnlyCollection<Category> categoriesForGeneration =
                await _categoryRepository.Get(post.Categories.Select(x => x.UrlName).ToArray());
            IReadOnlyCollection<Category> categoriesForSidebar = await _categoryRepository.Get();

            IReadOnlyCollection<Archive> archives = _archiveListBuilder.FromPostingTimes(postingTimes);

            await GenerateSidebar(postingTimes, archives, categoriesForSidebar);
            await GenerateArchive(post, postingTimes);
            await GenerateCategories(categoriesForGeneration);

            string homepage = await _homepageContentRenderer.Render(postingTimes.Take(HomepagePostsToShow).Select(x => x.UrlName).ToArray());
            await _outputRepository.SaveHomepageContent(homepage);
        }

        private async Task GenerateArchive(Post post, IReadOnlyCollection<PostingTime> postingTimes)
        {
            string archive = await _archiveRenderer.Render(
                post.PostedAtUtc,
                postingTimes);
            await _outputRepository.SaveArchive(post.PostedAtUtc.Year, post.PostedAtUtc.Month, archive);
        }

        private async Task GenerateCategories(IReadOnlyCollection<Category> categories)
        {
            foreach (Category category in categories)
            {
                string htmlSnippet = await _categoryRenderer.Render(category);
                await _outputRepository.SaveCategory(category.UrlName, htmlSnippet);
            }            
        }

        private async Task GenerateSidebar(IReadOnlyCollection<PostingTime> postingTimes, IReadOnlyCollection<Archive> archives,
            IReadOnlyCollection<Category> categories)
        {
            string sidebar = await _sidebarRenderer.Render(
                postingTimes.Take(RecentPostsToShow).ToArray(),
                archives,
                categories);
            await _outputRepository.SaveSidebar(sidebar);
        }
    }
}
