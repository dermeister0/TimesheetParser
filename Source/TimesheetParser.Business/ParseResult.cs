using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimesheetParser.Business
{
    using Model;

    public class ParseResult
    {
        public List<Job> Jobs { get; set; } = new List<Job>();
        public List<string> WrongLines { get; set; } = new List<string>();

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
                sb.AppendLine(job.TimeDescription);
                sb.AppendLine(job.Description);
                sb.AppendLine();
            }

            var totalMinutes = jobs.Sum(j => j.Duration.TotalMinutes);
            sb.AppendFormat("Total duration: {0:0.##'h'}\n", TimeSpan.FromMinutes(totalMinutes).TotalHours);
            sb.AppendLine("---------------------");
            sb.AppendLine();
        }
    }
}