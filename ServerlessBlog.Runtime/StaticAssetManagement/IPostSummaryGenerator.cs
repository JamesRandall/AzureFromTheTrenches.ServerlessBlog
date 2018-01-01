namespace ServerlessBlog.Runtime.StaticAssetManagement
{
    internal interface IPostSummaryGenerator
    {
        string[] ExtractSummaries(string[] titles, string[] textForSummaries);
    }
}