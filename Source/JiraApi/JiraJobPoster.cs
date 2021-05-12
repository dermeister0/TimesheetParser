using Heavysoft.TimesheetParser.PluginInterfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JiraApi
{
    class JiraJobPoster : IJobPoster
    {
        private readonly string apiUrl;

        public JiraJobPoster(string apiUrl)
        {
            this.apiUrl = apiUrl;
        }

        public async Task<bool> SendAsync(JobDefinition job, HttpClient client)
        {
            var url = apiUrl + $"issue/{job.TaskId}/worklog";
            var body = new WorkLog() { timeSpent = $"{job.Duration}m", comment = job.Description, started = job.Date.ToString("yyyy-MM-ddTHH:mm:ss.fffzz00") };

            var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var content = JsonConvert.DeserializeObject<WorkLogResponse>(await response.Content.ReadAsStringAsync());
                var id = content.id; // @@

                return true;
            }

            return false;
        }
    }
}
