using Heavysoft.TimesheetParser.PluginInterfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JiraApi
{
    class TempoJobPoster : IJobPoster
    {
        private readonly string jiraAccountId;
        private readonly string token;
        private readonly string jobType;

        public TempoJobPoster(string jiraAccountId, string token, string jobType)
        {
            this.jiraAccountId = jiraAccountId;
            this.token = token;
            this.jobType = jobType;
        }

        public async Task<bool> SendAsync(JobDefinition job, HttpClient client)
        {
            var url = "https://api.tempo.io/core/3/worklogs";
            var body = new TempoWorkLog() { issueKey = job.TaskId, timeSpentSeconds = job.Duration * 60, description = job.Description,
                startDate = job.Date.ToString("yyyy-MM-dd"), startTime = "00:00:00", authorAccountId = jiraAccountId };
            body.attributes.Add(new TempoWorkAttribute { key = "_JobType_", value = jobType });

            var authValue = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Authorization = authValue;

            var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var content = JsonConvert.DeserializeObject<TempoWorkLogResponse>(await response.Content.ReadAsStringAsync());
                var id = content.JiraWorklogId; // @@

                return true;
            }

            return false;
        }
    }
}
