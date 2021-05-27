using System;
using System.Linq;

namespace TimesheetParser.Business.IdleStrategies
{
    class ProportionalIdleStrategy : BaseIdleStrategy
    {
        public override void DistributeIdle(ParseResult result)
        {
            var idleJobs = GetIdleJobs(result);
            var idleTime = idleJobs.Sum(j => j.Duration.TotalMinutes);

            foreach (var idleJob in idleJobs)
            {
                result.Jobs.Remove(idleJob);
            }

            var normalJobs = result.Jobs.Where(j => !string.IsNullOrEmpty(j.Task)).ToList();
            var normalTime = normalJobs.Sum(j => j.Duration.TotalMinutes);

            foreach (var job in normalJobs)
            {
                var ratio = job.Duration.TotalMinutes / normalTime;
                job.ExtraTime = TimeSpan.FromMinutes(idleTime * ratio);

                if (job.Duration == TimeSpan.Zero && job.ExtraTime == TimeSpan.Zero)
                {
                    job.ExtraTime = TimeSpan.FromMinutes(1);
                }
            }
        }
    }
}
