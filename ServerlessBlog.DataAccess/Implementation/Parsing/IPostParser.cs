using ServerlessBlog.Model;

namespace ServerlessBlog.DataAccess.Implementation.Parsing
{
    internal interface IPostParser
    {
        Post FromString(string post, string defaultAuthor);
    }
}