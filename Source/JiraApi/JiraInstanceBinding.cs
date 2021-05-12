using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace JiraApi
{
    internal class JiraInstanceBinding
    {
        public string ProjectRegex { get; set; }

        [JsonIgnore]
        public Regex CachedRegex { get; set; }

        public string JiraInstance { get; set; }
    }
}
