using System;
using ServerlessBlog.Model;

namespace ServerlessBlog.Functions.AWSLambda
{
    internal static class ConfigurationOptionsFactory
    {
        public static ConfigurationOptions Create()
        {
            ConfigurationOptions options = new ConfigurationOptions
            {
                BlogName = Environment.GetEnvironmentVariable("BlogName"),
                AzureStorageAccountConnectionString = Environment.GetEnvironmentVariable("BlogStorage"),
                DefaultAuthor = Environment.GetEnvironmentVariable("DefaultAuthor"),
                StylesheetUrl = Environment.GetEnvironmentVariable("StylesheetUrl"),
                FavIconUrl = Environment.GetEnvironmentVariable("FavIconUrl"),
                BlogDomain = Environment.GetEnvironmentVariable("BlogDomain")
            };
            return options;
        }
    }
}
