using System.Collections.Generic;
using ServerlessBlog.Model;

namespace ServerlessBlog.Runtime.StaticAssetManagement
{
    internal interface IArchiveListBuilder
    {
        IReadOnlyCollection<Archive> FromPostingTimes(IEnumerable<PostingTime> postingTimes);
    }
}