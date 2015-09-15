using System;

namespace TimesheetParser
{
    internal class Job
    {
        private const string DurationFormat = "{0:h:mm tt} - {1:h:mm tt} @ {2:hh}:{2:mm}";
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

        public string TimeDescription => string.Format(DurationFormat, StartTime, EndTime, Duration);
    }
}