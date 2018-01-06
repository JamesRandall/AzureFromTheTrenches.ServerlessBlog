namespace ServerlessBlog.DataAccess.Implementation
{
    internal interface IBlobStoreFactory
    {
        IBlobStore Create(string folder);
    }
}