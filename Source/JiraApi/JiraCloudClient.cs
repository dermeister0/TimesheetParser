using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Heavysoft.TimesheetParser.PluginInterfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace JiraApi
{
    public class JiraCloudClient : ICrm
    {
        private const string ApiUrl = "https://saritasa.atlassian.net/rest/api/2/";

        private string login;
        private string password;
        private readonly Regex taskRegex = new Regex(@"^[A-z0-9]+-\d+$");

        public string GetName()
        {
            return "JIRA Cloud";
        }

        public bool IsValidTask(string taskId)
        {
            return taskRegex.IsMatch(taskId);
        }

        private HttpClient GetClient()
        {
            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}")));
            var client = new HttpClient { DefaultRequestHeaders = { Authorization = authValue } };
            return client;
        }

        public async Task<bool> Login(string login, string password)
        {
            this.login = login;
            this.password = password;

            using (var client = GetClient())
            {
                var response = await client.GetAsync(ApiUrl + "myself");
                return response.IsSuccessStatusCode;
            }
        }

        public Task<bool> Login(string token)
        {
            return Task.FromResult(false);
        }

        public async Task<bool> AddJob(JobDefinition job)
        {
            var url = ApiUrl + $"issue/{job.TaskId}/worklog";
            var body = new WorkLog() { timeSpent = $"{job.Duration}m", comment = job.Description, started = job.Date.ToString("yyyy-MM-ddTHH:mm:ss.fffzz00") };

            using (var client = GetClient())
            {
                var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var content = JsonConvert.DeserializeObject<WorkLogResponse>(await response.Content.ReadAsStringAsync());
                    var id = content.id; // @@

                    return true;
                }
            }

            return false;
        }

        public async Task<TaskHeader> GetTaskHeader(string taskId)
        {
            var url = ApiUrl + $"issue/{taskId}?fields=summary";
            var title = string.Empty;

            using (var client = GetClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = JsonConvert.DeserializeObject<IssueSummaryResponse>(await response.Content.ReadAsStringAsync());
                    title = content.fields["summary"];
                }
            }

            return new TaskHeader() { Title = title };
        }
    }
}
