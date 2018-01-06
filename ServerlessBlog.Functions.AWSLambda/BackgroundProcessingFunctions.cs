using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;

namespace ServerlessBlog.Functions.AWSLambda
{
    public class BackgroundProcessingFunctions
    {
        private readonly AmazonS3Client _s3Client;

        public BackgroundProcessingFunctions()
        {
            _s3Client = new AmazonS3Client();
        }

        public async Task ProcessPost(S3Event ev, ILambdaContext context)
        {
            foreach (var record in ev.Records)
            {
                context.Logger.LogLine(record.S3.Object.Key);
            }
            await Task.Delay(100);
        }
    }
}
