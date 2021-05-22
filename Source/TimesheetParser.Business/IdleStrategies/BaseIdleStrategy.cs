using System;
using System.Collections.Generic;
using System.Linq;

namespace TimesheetParser.Business.IdleStrategies
{
    abstract class BaseIdleStrategy : IIdleStrategy
    {
        public abstract void DistributeIdle(ParseResult result);

        protected IList<Model.Job> GetIdleJobs(ParseResult result)
        {
            return result.Jobs.Where(j => string.Compare(j.Description.Trim(), "Idle.", StringComparison.OrdinalIgnoreCase) == 0).ToList();
        }
    }
}
