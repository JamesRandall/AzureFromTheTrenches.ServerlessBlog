using ServerlessBlog.DataAccess.Implementation;
using ServerlessBlog.DataAccess.Implementation.Parsing;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess
{
    public class Factory
    {
        private readonly bool _useLocalFileSystem;
        private readonly string _defaultAuthor;
        private readonly string _storageAccount;
        
        public static Factory Instance { get; private set; }

        private Factory(IConfigurationOptions configuration, bool useLocalFileSystem)
        {
            _useLocalFileSystem = useLocalFileSystem;
            _defaultAuthor = configuration.DefaultAuthor;
            _storageAccount = configuration.StorageAccountConnectionString;
        }

        public static void Create(IConfigurationOptions configuration, bool useLocalFileSystem = false)
        {
            Instance = new Factory(configuration, useLocalFileSystem);
        }

        public IPostRepository GetPostRepository()
        {
            if (_useLocalFileSystem)
            {
                return new FileSystemPostRepository(new PostParser(new CategoryParser()), _defaultAuthor);
            }
            return new PostRepository(_storageAccount, new PostParser(new CategoryParser()), _defaultAuthor);
        }

        public ICategoryRepository GetCategoryRepository() => new CategoryRepository(StorageAccountConnectionString, new CategoryListBuilder());

        public IPostingTimeRepository GetPostingTimeRepository() => new PostingTimeRepository(StorageAccountConnectionString);

        public ITemplateRepository GetTemplateRepository() => new TemplateRepository(StorageAccountConnectionString);

        public IOutputRepository GetOutputRepository() => new OutputRepository(StorageAccountConnectionString);

        private string StorageAccountConnectionString =>
            _useLocalFileSystem ? "UseDevelopmentStorage=true" : _storageAccount;        
    }
}
