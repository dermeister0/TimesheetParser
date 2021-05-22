using System;
using System.Text.RegularExpressions;
using System.Linq;
using TimesheetParser.Business.Model;
using TimesheetParser.Business.IdleStrategies;

namespace TimesheetParser.Business
{
    public class Parser
    {
        IIdleStrategy idleStrategy;

        public Parser(IIdleStrategy idleStrategy)
        {
            this.idleStrategy = idleStrategy;
        }

        public ParseResult Parse(string source, bool distributeIdle)
        {
            var result = new ParseResult();

            if (string.IsNullOrEmpty(source))
                return result;

            var lines = source.Split(new[] { '\r', '\n', '\v' }, StringSplitOptions.RemoveEmptyEntries);
            Job currentJob = null;

            var taskRegex = new Regex(@"^\s*#((?:[A-z0-9]+-)?\d+)\s*$");
            var timeRegex1 = new Regex(@"^\s*\d+:\d+ [AP]M\s*$");
            var timeRegex2 = new Regex(@"^\s*\d+:\d+\s*$");

            var state = ParserState.Begin;

            foreach (var l in lines)
            {
                var line = l.Trim();
                if (string.IsNullOrEmpty(line))
                    continue;

                var taskMatch = taskRegex.Match(line);
                var timeMatch1 = timeRegex1.Match(line);
                var timeMatch2 = timeRegex2.Match(line);

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
                else if (timeMatch1.Success || timeMatch2.Success)
                {
                    var time = DateTime.Parse(line);
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
                idleStrategy.DistributeIdle(result);
                var et = result.Jobs.Sum(j => j.ExtraTime.TotalMinutes);
            }

            result.Jobs.Sort((j1, j2) => string.Compare(j1.Task, j2.Task));

            return result;
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