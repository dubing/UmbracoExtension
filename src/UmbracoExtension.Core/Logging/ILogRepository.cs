using System;
using UmbracoExtension.Core.Common;


namespace UmbracoExtension.Core.Logging
{
    public interface ILogRepository
    {
        PagingData<Log> GetLogs(DateTime? startTime, DateTime? endTime, PagingCriteria pagingCriteria);
    }
}
