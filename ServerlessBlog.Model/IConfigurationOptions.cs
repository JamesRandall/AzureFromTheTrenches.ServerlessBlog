namespace ServerlessBlog.Model
{
    public interface IConfigurationOptions
    {
        string AzureStorageAccountConnectionString { get; set; }
        string DefaultAuthor { get; set; }
        string BlogName { get; set; }
        string StylesheetUrl { get; set; }
        string FavIconUrl { get; set; }
        string BlogDomain { get; set; }
        string S3Bucket { get; set; }
    }
}