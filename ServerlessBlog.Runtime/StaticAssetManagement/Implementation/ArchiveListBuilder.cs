using System;
using System.Collections.Generic;
using System.Linq;
using ServerlessBlog.Model;

namespace ServerlessBlog.Runtime.StaticAssetManagement.Implementation
{
    internal class ArchiveListBuilder : IArchiveListBuilder
    {
        public IReadOnlyCollection<Archive> FromPostingTimes(IEnumerable<PostingTime> postingTimes)
        {
            return postingTimes.GroupBy(x => new DateTime(x.PostedAtUtc.Year, x.PostedAtUtc.Month, 1, 0, 0, 0))
                .Select(x => new Archive
                { 
                    DisplayName = x.Key.ToString(Constants.ArchiveStringFormat),
                    Month = x.Key.Month,
                    Year = x.Key.Year
                })
                .ToArray();
        }
    }
}
