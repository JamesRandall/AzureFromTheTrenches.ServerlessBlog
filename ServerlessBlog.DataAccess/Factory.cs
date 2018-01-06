using ServerlessBlog.DataAccess.Implementation;
using ServerlessBlog.DataAccess.Implementation.Azure;
using ServerlessBlog.DataAccess.Implementation.Parsing;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess
{
    public class Factory
    {
        private readonly CloudVendorEnum _cloudVendor;

        public enum CloudVendorEnum
        {
            Aws,
            Azure
        }
        
        private readonly string _defaultAuthor;
        private readonly string _storageAccount;
        
        public static Factory Instance { get; private set; }

        private Factory(IConfigurationOptions configuration, CloudVendorEnum cloudVendor)
        {
            _cloudVendor = cloudVendor;
            _defaultAuthor = configuration.DefaultAuthor;
            _storageAccount = configuration.StorageAccountConnectionString;
        }

        public static void Create(IConfigurationOptions configuration, CloudVendorEnum cloudVendor)
        {
            Instance = new Factory(configuration, cloudVendor);
        }

        public IPostRepository GetPostRepository()
        {
            return new PostRepository(GetBlobStoreFactory(), new PostParser(new CategoryParser()), _defaultAuthor);
        }

        public ICategoryRepository GetCategoryRepository() => new AzureCategoryRepository(StorageAccountConnectionString, new CategoryListBuilder());

        public IPostingTimeRepository GetPostingTimeRepository() => new AzurePostingTimeRepository(StorageAccountConnectionString);

        public ITemplateRepository GetTemplateRepository() => new TemplateRepository(GetBlobStoreFactory());

        public IOutputRepository GetOutputRepository() => new OutputRepository(StorageAccountConnectionString);

        internal IBlobStoreFactory GetBlobStoreFactory() => new AzureBlobStoreFactory(_storageAccount);

        //private string StorageAccountConnectionString => _useLocalFileSystem ? "UseDevelopmentStorage=true" : _storageAccount;        
    }
}
