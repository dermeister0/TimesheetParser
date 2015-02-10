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

        public TimeSpan Duration => EndTime - StartTime;

        public string TimeDescription => string.Format(DurationFormat, StartTime, EndTime, Duration);
    }
}