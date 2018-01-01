namespace ServerlessBlog.Model
{
    public class ConfigurationOptions : IConfigurationOptions
    {
        public string StorageAccountConnectionString { get; set; }

        public string DefaultAuthor { get; set; }

        public string BlogName { get; set; }

        public string StylesheetUrl { get; set; }

        public string FavIconUrl { get; set; }
        public string BlogDomain { get; set; }
    }
}
