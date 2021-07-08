﻿using System;

namespace TimesheetParser.Business.Model
{
    public class Job
    {
        private const string TimeDescriptionFormat = "{0:h:mm tt} - {1:h:mm tt} @ {2}";
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Task { get; set; }
        public string Description { get; set; }
        public TimeSpan ExtraTime { get; set; }
        public int JobId { get; set; }

        public TimeSpan Duration
        {
            get
            {
                var duration = EndTime - StartTime + ExtraTime;
                return duration.TotalMinutes > 0 ? duration : TimeSpan.FromMinutes(1);
            }
        }

        public string GetTimeDescription(DurationFormat durationFormat)
        {
            return string.Format(TimeDescriptionFormat, StartTime, EndTime, durationFormat == Business.DurationFormat.Hours 
                ? Duration.TotalHours.ToString("0.##'h'") 
                : Duration.ToString("hh':'mm"));
        }
    }
}