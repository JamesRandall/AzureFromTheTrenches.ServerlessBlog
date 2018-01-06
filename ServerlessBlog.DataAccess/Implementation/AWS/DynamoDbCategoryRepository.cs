using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ServerlessBlog.DataAccess.Implementation.Model;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation.AWS
{
    internal class DynamoDbCategoryRepository : ICategoryRepository
    {
        private readonly ICategoryListBuilder _categoryListBuilder;
        private readonly AmazonDynamoDBClient _client;

        public DynamoDbCategoryRepository(ICategoryListBuilder categoryListBuilder)
        {
            _categoryListBuilder = categoryListBuilder;
            _client = new AmazonDynamoDBClient();
        }

        public async Task<IReadOnlyCollection<Category>> Get()
        {
            List<CategoryItem> categoryItems = new List<CategoryItem>();
            ScanResponse response = null;
            //do
            //{
                // grubby hack to get things working quickly, reuses azure table, will resolve
                response = await _client.ScanAsync("categories", new Dictionary<string, Condition>());
                categoryItems.AddRange(response.Items.Select(item => new CategoryItem
                {
                    PostedAtUtc = DateTime.ParseExact(item["PostedAtUtc"].S, "O", CultureInfo.InvariantCulture),
                    PartitionKey = item["UrlName"].S,
                    PostTitle = item["Title"].S,
                    DisplayName = item["DisplayName"].S,
                    RowKey = item["PostUrlName"].S
                }));
            //} while (response != null && response.Count > 0);

            return _categoryListBuilder.FromCategoryItems(categoryItems);
        }

        public async Task<IReadOnlyCollection<Category>> Get(params string[] categoryUrlNames)
        {
            // come back and scan
            IReadOnlyCollection<Category> all = await Get();
            return all.Where(x => categoryUrlNames.Contains(x.UrlName)).ToArray();
        }

        public async Task InsertOrUpdate(Post post)
        {
            foreach (Category category in post.Categories)
            {
                await _client.PutItemAsync("categories", new Dictionary<string, AttributeValue>
                {
                    {"UrlName", new AttributeValue {S = category.UrlName}},
                    {"DisplayName", new AttributeValue{ S=category.DisplayName} },
                    {"PostedAtUtc", new AttributeValue {S = post.PostedAtUtc.ToString("O")}},
                    {"Title", new AttributeValue {S = post.Title}},
                    {"PostUrlName", new AttributeValue{S = post.UrlName} }
                });
            }            
        }
    }
}
