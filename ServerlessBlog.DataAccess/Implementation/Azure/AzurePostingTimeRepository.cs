using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessBlog.DataAccess.Implementation.Model;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation.Azure
{
    internal class AzurePostingTimeRepository : IPostingTimeRepository
    {
        private const string TableName = "postingtimes";
        private readonly CloudTable _table;

        public AzurePostingTimeRepository(string storageAccountConnectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(TableName);
        }

        public async Task<IReadOnlyCollection<PostingTime>> Get()
        {
            TableContinuationToken token = null;
            List<PostedAtIndexItem> items = new List<PostedAtIndexItem>();
            do
            {
                var queryResult = await _table.ExecuteQuerySegmentedAsync(new TableQuery<PostedAtIndexItem>(), token);
                items.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return items.Select(x => new PostingTime
            {
                PostedAtUtc = x.PostedAtUtc,
                UrlName = x.PostUrlName,
                Title = x.Title
            }).OrderByDescending(x => x.PostedAtUtc).ToList();
        }

        public async Task InsertOrUpdate(Post post)
        {
            PostedAtIndexItem item = new PostedAtIndexItem
            {
                PartitionKey = PostedAtIndexItem.GetPartitionKey(post),
                RowKey = PostedAtIndexItem.GetRowKey(),
                PostedAtUtc = post.PostedAtUtc,
                Title = post.Title
            };
            await _table.ExecuteAsync(TableOperation.InsertOrReplace(item));
        }
    }
}
