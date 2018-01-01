using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServerlessBlog.Model;

namespace ServerlessBlog.Runtime.StaticAssetManagement
{
    internal interface IArchiveRenderer
    {
        Task<string> Render(DateTime forDate, IReadOnlyCollection<PostingTime> postingTimes);
    }
}