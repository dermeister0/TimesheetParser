using System.Collections.Generic;

namespace JiraApi
{
    class TempoWorkLog
    {
        public string issueKey { get; set; }

        public int timeSpentSeconds { get; set; }

        public string description { get; set; }

        public string authorAccountId { get; set; }

        public string startDate { get; set; }

        public string startTime { get; set; }

        public List<TempoWorkAttribute> attributes { get; set; } = new List<TempoWorkAttribute>();
    }
}
