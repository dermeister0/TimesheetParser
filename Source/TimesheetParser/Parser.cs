using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TimesheetParser
{
    internal class Parser
    {
        public ParseResult Parse(string source)
        {
            var result = new ParseResult();

            var lines = source.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Job currentJob = null;

            var taskRegex = new Regex(@"#(\d+)");
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

            return result;
        }
    }
}