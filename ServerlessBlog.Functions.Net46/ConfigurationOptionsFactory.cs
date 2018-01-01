using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerlessBlog.Model;

namespace ServerlessBlog.Functions.Net46
{
    internal static class ConfigurationOptionsFactory
    {
        public static ConfigurationOptions Create()
        {
            ConfigurationOptions options = new ConfigurationOptions
            {
                BlogName = Environment.GetEnvironmentVariable("BlogName"),
                StorageAccountConnectionString = Environment.GetEnvironmentVariable("BlogStorage"),
                DefaultAuthor = Environment.GetEnvironmentVariable("DefaultAuthor"),
                StylesheetUrl = Environment.GetEnvironmentVariable("StylesheetUrl"),
                FavIconUrl = Environment.GetEnvironmentVariable("FavIconUrl"),
                BlogDomain = Environment.GetEnvironmentVariable("BlogDomain")
            };
            return options;
        }
    }
}
