using System;
using System.Linq;

namespace TimesheetParser.Business.IdleStrategies
{
    internal class ProportionalIdleStrategy : BaseIdleStrategy
    {
        public override void DistributeIdle(ParseResult result)
        {
            var idleJobs = GetIdleJobs(result);
            var idleTime = idleJobs.Sum(j => j.TotalMinutes);

            foreach (var idleJob in idleJobs)
            {
                result.Jobs.Remove(idleJob);
            }

            var normalJobs = result.Jobs.Where(j => !string.IsNullOrEmpty(j.Task)).ToList();
            var normalTime = normalJobs.Sum(j => j.TotalMinutes);

            foreach (var job in normalJobs)
            {
                var ratio = (double)job.TotalMinutes / normalTime;
                job.ExtraTime = TimeSpan.FromMinutes(idleTime * ratio);
            }
        }
    }
}
