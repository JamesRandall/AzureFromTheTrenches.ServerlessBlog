using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation.Parsing
{
    internal interface ICategoryParser
    {
        Category FromString(string categoryString);
    }
}