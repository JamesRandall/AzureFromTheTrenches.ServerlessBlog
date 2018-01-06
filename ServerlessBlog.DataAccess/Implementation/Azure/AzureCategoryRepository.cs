using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ServerlessBlog.DataAccess.Implementation.Model;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation.Azure
{
    internal class AzureCategoryRepository : ICategoryRepository
    {
        private readonly ICategoryListBuilder _categoryListBuilder;
        private const string TableName = "categories";
        private readonly CloudTable _table;

        public AzureCategoryRepository(string storageAccountConnectionString, ICategoryListBuilder categoryListBuilder)
        {
            _categoryListBuilder = categoryListBuilder;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(TableName);
        }

        public async Task<IReadOnlyCollection<Category>> Get()
        {
            TableContinuationToken token = null;
            List<CategoryItem> items = new List<CategoryItem>();
            do
            {
                var queryResult = await _table.ExecuteQuerySegmentedAsync(new TableQuery<CategoryItem>(), token);
                items.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return _categoryListBuilder.FromCategoryItems(items);
        }

        public async Task<IReadOnlyCollection<Category>> Get(params string[] categoryUrlNames)
        {
            TableContinuationToken token = null;
            List<CategoryItem> items = new List<CategoryItem>();

            foreach (string categoryUrlName in categoryUrlNames)
            {
                TableQuery<CategoryItem> query = new TableQuery<CategoryItem>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, categoryUrlName));
                do
                {
                    var queryResult = await _table.ExecuteQuerySegmentedAsync(query, token);
                    items.AddRange(queryResult.Results);
                    token = queryResult.ContinuationToken;
                } while (token != null);
            }
            
            return _categoryListBuilder.FromCategoryItems(items);
        }

        public async Task InsertOrUpdate(Post post)
        {
            foreach (Category category in post.Categories)
            {
                CategoryItem item = new CategoryItem
                {
                    DisplayName = category.DisplayName,
                    PartitionKey = CategoryItem.GetPartitionKey(category.UrlName),
                    PostedAtUtc = post.PostedAtUtc,
                    PostTitle = post.Title,
                    RowKey = CategoryItem.GetRowKey(post.UrlName)
                };
                await _table.ExecuteAsync(TableOperation.InsertOrReplace(item));
            }
        }
    }
}
