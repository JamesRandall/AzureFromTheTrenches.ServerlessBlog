using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessBlog.DataAccess.Implementation.Model
{
    internal class CategoryItem : TableEntity
    {
        public string UrlName => PartitionKey;

        public string PostUrlName => RowKey;

        public string DisplayName { get; set; }

        public string PostTitle { get; set; }

        public DateTime PostedAtUtc { get; set; }

        public static string GetPartitionKey(string categoryUrlName)
        {
            return categoryUrlName;
        }

        public static string GetRowKey(string postUrlName)
        {
            return postUrlName;
        }
    }
}
