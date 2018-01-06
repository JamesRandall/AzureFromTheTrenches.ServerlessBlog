namespace ServerlessBlog.DataAccess.Implementation.AWS
{
    class S3BlobStoreFactory : IBlobStoreFactory
    {
        private readonly string _bucket;

        public S3BlobStoreFactory(string bucket)
        {
            _bucket = bucket;
        }

        public IBlobStore Create(string folder)
        {
            return new S3BlobStore(_bucket, folder);
        }
    }
}
