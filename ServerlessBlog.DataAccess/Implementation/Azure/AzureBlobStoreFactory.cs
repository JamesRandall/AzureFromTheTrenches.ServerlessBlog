namespace ServerlessBlog.DataAccess.Implementation.Azure
{
    internal class AzureBlobStoreFactory : IBlobStoreFactory
    {
        private readonly string _connectionString;

        public AzureBlobStoreFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IBlobStore Create(string folder)
        {
            return new AzureBlobStore(_connectionString, folder);
        }
    }
}
