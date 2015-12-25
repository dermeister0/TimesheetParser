using System;
using System.Text.RegularExpressions;
using System.Linq;
using TimesheetParser.Business.Model;

namespace TimesheetParser.Business
{
    public class Parser
    {
        public ParseResult Parse(string source, bool distributeIdle)
        {
            var result = new ParseResult();

            if (string.IsNullOrEmpty(source))
                return result;

            var lines = source.Split(new[] { '\r', '\n', '\v' }, StringSplitOptions.RemoveEmptyEntries);
            Job currentJob = null;

            var taskRegex = new Regex(@"#((?:[A-z0-9]+-)?\d+)");
            var timeRegex = new Regex(@"\d+:\d+ [AP]M");

            var state = ParserState.Begin;

            foreach (var l in lines)
            {
                var line = l.Trim();
                if (string.IsNullOrEmpty(line))
                    continue;

                var taskMatch = taskRegex.Match(line);
                var timeMatch = timeRegex.Match(line);

                if (taskMatch.Success)
                {
                    if (currentJob == null)
                    {
                        result.WrongLines.Add(line);
                        continue;
                    }

                    if (state == ParserState.EndTimeFound)
                    {
                        AddJob(result, currentJob);
                        currentJob = new Job { StartTime = currentJob.EndTime };
                    }

                    currentJob.Task = taskMatch.Groups[1].Value;
                    state = ParserState.TaskFound;
                }
                else if (timeMatch.Success)
                {
                    var time = DateTime.Parse(timeMatch.Groups[0].Value);
                    if (currentJob != null)
                    {
                        currentJob.EndTime = time;
                        state = ParserState.EndTimeFound;
                    }
                    else
                    {
                        currentJob = new Job { StartTime = time };
                        state = ParserState.StartTimeFound;
                    }
                }
                else
                {
                    if (currentJob == null)
                    {
                        result.WrongLines.Add(line);
                        continue;
                    }

                    if (state == ParserState.EndTimeFound)
                    {
                        AddJob(result, currentJob);
                        currentJob = new Job { StartTime = currentJob.EndTime };
                    }

                    currentJob.Description += line;
                    state = ParserState.DescriptionFound;
                }
            }

            if (state == ParserState.EndTimeFound)
            {
                AddJob(result, currentJob);
            }

            if (distributeIdle)
            {
                DistributeIdle(result);
            }

            result.Jobs.Sort((j1, j2) => string.Compare(j1.Task, j2.Task));

            return result;
        }

        void DistributeIdle(ParseResult result)
        {
            var idleJobs = result.Jobs.Where(j => string.Compare(j.Description.Trim(), "Idle.", StringComparison.OrdinalIgnoreCase) == 0).ToList();
            var idleTime = idleJobs.Sum(j => j.Duration.TotalMinutes);

            foreach (var idleJob in idleJobs)
            {
                result.Jobs.Remove(idleJob);
            }

            var normalJobs = result.Jobs.Where(j => !string.IsNullOrEmpty(j.Task)).ToList();

            var additionalMinutes = Math.Floor(idleTime / normalJobs.Count);
            var additionalTime = TimeSpan.FromMinutes(additionalMinutes > 0 ? additionalMinutes : 1);

            foreach (var job in normalJobs)
            {
                job.ExtraTime = additionalTime;
            }
        }

        void AddJob(ParseResult result, Job job)
        {
            if (job.Description == null)
            {
                job.Description = "";
                result.WrongLines.Add(string.Format("Job without description: #{0} ({1} - {2})", job.Task, job.StartTime, job.EndTime));
            }

            result.Jobs.Add(job);
        }
    }
}