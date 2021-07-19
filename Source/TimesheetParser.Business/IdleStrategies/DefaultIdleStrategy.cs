using System;
using System.Linq;

namespace TimesheetParser.Business.IdleStrategies
{
    internal class DefaultIdleStrategy : BaseIdleStrategy
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

            var additionalMinutes = Math.Floor((double)idleTime / normalJobs.Count);
            var additionalTime = TimeSpan.FromMinutes(additionalMinutes > 0 ? additionalMinutes : 1);

            foreach (var job in normalJobs)
            {
                job.ExtraTime = additionalTime;
            }
        }
    }
}
