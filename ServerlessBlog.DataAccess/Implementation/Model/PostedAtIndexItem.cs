using System;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation.Model
{
    internal class PostedAtIndexItem : TableEntity
    {
        public string PostUrlName => PartitionKey;

        public DateTime PostedAtUtc { get; set; }

        public string Title { get; set; }

        public static string GetPartitionKey(Post post)
        {
            return post.UrlName;
        }

        
        public static string GetRowKey()
        {
            return "";
        }
    }
}
