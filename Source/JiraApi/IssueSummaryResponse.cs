using Newtonsoft.Json;
using System.Collections.Generic;

namespace JiraApi
{
    internal class IssueSummaryResponse
    {
        [JsonProperty("Fields")]
        public Dictionary<string, string> Fields { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }
    }
}
