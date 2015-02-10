using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimesheetParser
{
    internal class ParseResult
    {
        public List<Job> Jobs { get; set; } = new List<Job>();
        public List<string> WrongLines { get; set; } = new List<string>();

        private const string DurationFormat = "{0:h:mm tt} - {1:h:mm tt} @ {2:hh}:{2:mm}\n";

        public string Format()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Jobs");
            sb.AppendLine("====");
            sb.AppendLine();

            WriteJobs(Jobs.Where(j => !string.IsNullOrEmpty(j.Task)).ToList(), sb);

            sb.AppendLine("Jobs without task");
            sb.AppendLine("=================");

            WriteJobs(Jobs.Where(j => string.IsNullOrEmpty(j.Task)).ToList(), sb);

            sb.AppendLine("Wrong lines");
            sb.AppendLine("===========");

            foreach (var wrongLine in WrongLines)
            {
                sb.AppendLine(wrongLine);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        void WriteJobs(IReadOnlyCollection<Job> jobs, StringBuilder sb)
        {
            foreach (var job in jobs)
            {
                if (!string.IsNullOrEmpty(job.Task))
                    sb.AppendLine("#" + job.Task);
                sb.AppendFormat(DurationFormat, job.StartTime, job.EndTime, job.Duration);
                sb.AppendLine(job.Description);
                sb.AppendLine();
            }

            var totalMinutes = jobs.Sum(j => j.Duration.TotalMinutes);
            sb.AppendFormat("Total duration: {0:hh}:{0:mm}\n", TimeSpan.FromMinutes(totalMinutes));
            sb.AppendLine("---------------------");
            sb.AppendLine();
        }
    }
}