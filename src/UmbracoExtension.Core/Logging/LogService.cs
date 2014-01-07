using System;
using UmbracoExtension.Core.Common;
using Microsoft.Practices.Unity;

namespace UmbracoExtension.Core.Logging
{
    public class LogService
    {
        private ILogRepository _logRepo;

        [InjectionConstructor]
        public LogService(ILogRepository logRepo)
        {
            _logRepo = logRepo;
        }

        public PagingData<Log> GetLogs(DateTime? startTime, DateTime? endTime, PagingCriteria pagingCriteria)
        {
            return _logRepo.GetLogs(startTime, endTime, pagingCriteria);
        }
    }
}
