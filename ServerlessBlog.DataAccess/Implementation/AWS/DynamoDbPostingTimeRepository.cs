using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation.AWS
{
    internal class DynamoDbPostingTimeRepository : IPostingTimeRepository
    {
        private readonly AmazonDynamoDBClient _client;

        public DynamoDbPostingTimeRepository()
        {
            _client = new AmazonDynamoDBClient();
        }

        public async Task<IReadOnlyCollection<PostingTime>> Get()
        {
            List<PostingTime> postingTimes = new List<PostingTime>();
            ScanResponse response = null;
            //do
            //{
                response = await _client.ScanAsync("postingtimes", new Dictionary<string, Condition>());
                postingTimes.AddRange(response.Items.Select(item => new PostingTime
                {
                    PostedAtUtc = DateTime.ParseExact(item["PostedAtUtc"].S, "O", CultureInfo.InvariantCulture),
                    UrlName = item["UrlName"].S,
                    Title = item["Title"].S
                }));                
            //} while (response != null && response.Count > 0);
            return postingTimes;
        }

        public async Task InsertOrUpdate(Post post)
        {
            await _client.PutItemAsync("postingtimes", new Dictionary<string, AttributeValue>
            {
                {"UrlName", new AttributeValue {S = post.UrlName}},
                {"Title", new AttributeValue {S = post.Title}},
                {"PostedAtUtc", new AttributeValue {S = post.PostedAtUtc.ToString("O")}}
            });
        }
    }
}
