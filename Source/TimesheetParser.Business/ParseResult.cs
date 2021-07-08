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

        public string Format(DurationFormat durationFormat)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Jobs");
            sb.AppendLine("====");
            sb.AppendLine();

            WriteJobs(Jobs.Where(j => !string.IsNullOrEmpty(j.Task)).ToList(), sb, durationFormat);

            sb.AppendLine("Jobs without task");
            sb.AppendLine("=================");

            WriteJobs(Jobs.Where(j => string.IsNullOrEmpty(j.Task)).ToList(), sb, durationFormat);

            sb.AppendLine("Wrong lines");
            sb.AppendLine("===========");

            foreach (var wrongLine in WrongLines)
            {
                sb.AppendLine(wrongLine);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        void WriteJobs(IReadOnlyCollection<Job> jobs, StringBuilder sb, DurationFormat durationFormat)
        {
            foreach (var job in jobs)
            {
                if (!string.IsNullOrEmpty(job.Task))
                    sb.AppendLine("#" + job.Task);
                sb.AppendLine(job.GetTimeDescription(durationFormat));
                sb.AppendLine(job.Description);
                sb.AppendLine();
            }

            var totalMinutes = jobs.Sum(j => j.Duration.TotalMinutes);
            if (durationFormat == DurationFormat.Hours)
            {
                sb.AppendFormat("Total duration: {0:0.##'h'}\n", TimeSpan.FromMinutes(totalMinutes).TotalHours);
            }
            else
            {
                sb.AppendFormat("Total duration: {0:hh':'mm}\n", TimeSpan.FromMinutes(totalMinutes));
            }
            sb.AppendLine("---------------------");
            sb.AppendLine();
        }
    }
}