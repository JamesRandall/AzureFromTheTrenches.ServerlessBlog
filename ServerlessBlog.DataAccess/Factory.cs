using ServerlessBlog.DataAccess.Implementation;
using ServerlessBlog.DataAccess.Implementation.AWS;
using ServerlessBlog.DataAccess.Implementation.Azure;
using ServerlessBlog.DataAccess.Implementation.Parsing;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess
{
    public class Factory
    {
        private readonly CloudVendorEnum _cloudVendor;

        private readonly string _defaultAuthor;
        private readonly string _azureStorageAccountConnectionString;
        private readonly string _s3Bucket;


        public static Factory Instance { get; private set; }

        private Factory(IConfigurationOptions configuration, CloudVendorEnum cloudVendor)
        {
            _cloudVendor = cloudVendor;
            _defaultAuthor = configuration.DefaultAuthor;
            _azureStorageAccountConnectionString = configuration.AzureStorageAccountConnectionString;
            _s3Bucket = configuration.S3Bucket;
        }

        public static void Create(IConfigurationOptions configuration, CloudVendorEnum cloudVendor)
        {
            Instance = new Factory(configuration, cloudVendor);
        }

        public ICategoryRepository GetCategoryRepository() => new AzureCategoryRepository(_azureStorageAccountConnectionString, new CategoryListBuilder());

        public IPostingTimeRepository GetPostingTimeRepository() => new AzurePostingTimeRepository(_azureStorageAccountConnectionString);

        public IPostRepository GetPostRepository() => new PostRepository(GetBlobStoreFactory(), new PostParser(new CategoryParser()), _defaultAuthor);

        public ITemplateRepository GetTemplateRepository() => new TemplateRepository(GetBlobStoreFactory());

        public IOutputRepository GetOutputRepository() => new OutputRepository(GetBlobStoreFactory());

        internal IBlobStoreFactory GetBlobStoreFactory() => _cloudVendor == CloudVendorEnum.Azure ? 
            (IBlobStoreFactory)new AzureBlobStoreFactory(_azureStorageAccountConnectionString) :
            new S3BlobStoreFactory(_s3Bucket);
    }
}
