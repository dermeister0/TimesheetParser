using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TimesheetParser.Business.IdleStrategies
{
    abstract class BaseIdleStrategy : IIdleStrategy
    {
        Regex idleRegex = new Regex(@"(?i)^\s?idle\.?\s?$", RegexOptions.Compiled);

        public abstract void DistributeIdle(ParseResult result);

        protected IList<Model.Job> GetIdleJobs(ParseResult result)
        {
            return result.Jobs.Where(j => idleRegex.IsMatch(j.Description.Trim())).ToList();
        }
    }
}
