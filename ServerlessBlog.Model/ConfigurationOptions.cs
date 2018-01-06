namespace ServerlessBlog.Model
{
    public class ConfigurationOptions : IConfigurationOptions
    {
        public ConfigurationOptions()
        {
            S3Bucket = "serverlessblobdev.azurefromthetrenches.com";
        }

        public string AzureStorageAccountConnectionString { get; set; }

        public string DefaultAuthor { get; set; }

        public string BlogName { get; set; }

        public string StylesheetUrl { get; set; }

        public string FavIconUrl { get; set; }
        public string BlogDomain { get; set; }

        public string S3Bucket { get; set; }
    }
}
