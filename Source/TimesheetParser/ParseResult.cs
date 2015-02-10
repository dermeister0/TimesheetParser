using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimesheetParser
{
    internal class ParseResult
    {
        public List<Job> Jobs { get; set; } = new List<Job>();
        public List<string> WrongLines { get; set; } = new List<string>();

        public string Format()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Jobs");
            sb.AppendLine("====");
            sb.AppendLine();

            foreach (var job in Jobs.Where(j => !string.IsNullOrEmpty(j.Task)))
            {
                sb.AppendLine("#" + job.Task);
                sb.AppendLine(job.Description);
                sb.AppendLine();
            }

            sb.AppendLine("Jobs without task");
            sb.AppendLine("=================");

            foreach (var job in Jobs.Where(j => string.IsNullOrEmpty(j.Task)))
            {
                sb.AppendLine(job.Description);
                sb.AppendLine();
            }

            sb.AppendLine("Wrong lines");
            sb.AppendLine("===========");

            foreach (var wrongLine in WrongLines)
            {
                sb.AppendLine(wrongLine);
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}