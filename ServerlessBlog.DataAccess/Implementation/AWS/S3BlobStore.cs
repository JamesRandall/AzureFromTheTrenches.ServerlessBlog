using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace ServerlessBlog.DataAccess.Implementation.AWS
{
    class S3BlobStore : IBlobStore
    {
        private readonly string _s3Bucket;
        private readonly string _folder;
        private readonly AmazonS3Client _client;

        public S3BlobStore(string s3Bucket, string folder)
        {
            _s3Bucket = s3Bucket;
            _folder = folder;
            _client = new AmazonS3Client();
        }

        public async Task<string> Get(string name)
        {
            string filename = $"{_folder}/{name}";
            using (GetObjectResponse response = await _client.GetObjectAsync(_s3Bucket, filename))
            using (Stream responseStream = response.ResponseStream)
            {
                StreamReader reader = new StreamReader(responseStream);
                string result = reader.ReadToEnd();
                return result;
            }
        }

        public async Task Save(string name, string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            using (MemoryStream inputStream = new MemoryStream(bytes))
            {
                string filename = $"{_folder}/{name}";
                PutObjectRequest request = new PutObjectRequest
                {
                    Key = filename,
                    BucketName = _s3Bucket,
                    CannedACL = S3CannedACL.Private,
                    InputStream = inputStream
                };
                await _client.PutObjectAsync(request);
            }
        }
    }
}
