using System;
using System.Collections.Generic;
using System.Linq;
using UmbracoExtension.Core.Common;


namespace UmbracoExtension.Core.Logging
{
    public class LogRepository : ILogRepository
    {
        public PagingData<Log> GetLogs(DateTime? startTime, DateTime? endTime, PagingCriteria pagingCriteria)
        {
            using (var ctx = new UmbracoExtensionDataContext())
            {
                IQueryable<Log> logs;

                if(startTime.HasValue && endTime.HasValue && startTime.Value > endTime.Value)
                {
                    logs = ctx.Logs.Where(t => t.Date >= startTime.Value && t.Date <= endTime.Value);
                }
                else
                {
                    logs = ctx.Logs;
                }

                int totalCount = logs.Count();

                return new PagingData<Log>() { Items = GetLimitLogs(logs, pagingCriteria), TotalCount = totalCount };
            }
        }

        private IEnumerable<Log> GetLimitLogs(IEnumerable<Log> logs, PagingCriteria pagingCriteria)
        {
            if (logs != null)
            {
                return logs
                    .OrderByDescending(x => x.Date)
                    .Skip(pagingCriteria.PageNumber * pagingCriteria.PageSize)
                    .Take(pagingCriteria.PageSize)
                    .ToList();
            }

            return null;
        }
    }
}
