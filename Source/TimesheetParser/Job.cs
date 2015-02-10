using System;

namespace TimesheetParser
{
    internal class Job
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Task { get; set; }
        public string Description { get; set; }
    }
}