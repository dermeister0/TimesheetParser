using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Heavysoft.TimesheetParser.PluginInterfaces;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace JiraApi
{
    public class JiraCloudClient : ICrm
    {
        private const string ApiUrl = "https://saritasa.atlassian.net/rest/api/2/";

        private string login;
        private string password;
        private readonly RestClient restClient;
        private readonly Regex taskRegex = new Regex(@"^[A-z0-9]+-\d+$");

        public JiraCloudClient()
        {
            restClient = new RestClient(ApiUrl);
        }

        public string GetName()
        {
            return "JIRA Cloud";
        }

        public bool IsValidTask(string taskId)
        {
            return taskRegex.IsMatch(taskId);
        }

        public async Task<bool> Login(string login, string password)
        {
            this.login = login;
            this.password = password;

            restClient.Authenticator = new HttpBasicAuthenticator(login, password);
            var request = new RestRequest("myself");
            var response = await restClient.Execute(request);

            return response.IsSuccess && response.StatusCode == HttpStatusCode.OK;
        }

        public Task<bool> Login(string token)
        {
            return Task.FromResult(false);
        }

        public async Task<bool> AddJob(JobDefinition job)
        {
            var request = new RestRequest("issue/{issueId}/worklog", Method.POST);
            request.AddUrlSegment("issueId", job.TaskId);
            request.AddBody(new WorkLog() { timeSpent = $"{job.Duration}m", comment = job.Description, started = job.Date.ToString("yyyy-MM-ddTHH:mm:ss.fffzz00") });

            var response = await restClient.Execute<WorkLogResponse>(request);
            if (response.IsSuccess && response.StatusCode == HttpStatusCode.Created)
            {
                var id = response.Data.id; // @@

                return true;
            }
            return false;
        }

        public async Task<TaskHeader> GetTaskHeader(string taskId)
        {
            var request = new RestRequest("issue/{issueId}?fields=summary");
            request.AddUrlSegment("issueId", taskId);

            var response = await restClient.Execute<IssueSummaryResponse>(request);

            var title = string.Empty;
            if (response.IsSuccess && response.StatusCode == HttpStatusCode.OK)
            {
                title = response.Data.fields["summary"];
            }

            return new TaskHeader() { Title = title };
        }
    }
}
