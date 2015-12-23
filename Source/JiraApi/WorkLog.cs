using System;

namespace JiraApi
{
    class WorkLog
    {
        public string timeSpent { get; set; }
        public DateTime started { get; set; }
        public string comment { get; set; }
    }
}
