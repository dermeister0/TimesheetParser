using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace TimesheetParser
{
    internal class Parser
    {
        public ParseResult Parse(string source, bool distributeIdle)
        {
            var result = new ParseResult();

            var lines = source.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Job currentJob = null;

            var taskRegex = new Regex(@"#([A-Z]+-)*(\d+)");
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
                        result.Jobs.Add(currentJob);
                        currentJob = new Job { StartTime = currentJob.EndTime };
                    }

                    currentJob.Task = taskMatch.Groups[0].Value;
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
                        result.Jobs.Add(currentJob);
                        currentJob = new Job { StartTime = currentJob.EndTime };
                    }

                    currentJob.Description += line;
                    state = ParserState.DescriptionFound;
                }
            }

            if (state == ParserState.EndTimeFound)
            {
                result.Jobs.Add(currentJob);
            }

            if (distributeIdle)
            {
                DistributeIdle(result);
            }

            return result;
        }

        void DistributeIdle(ParseResult result)
        {
            var idleJobs = result.Jobs.Where(j => string.Compare(j.Description.Trim(), "Idle.", true) == 0).ToList();
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
    }
}