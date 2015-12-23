using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Heavysoft.TimesheetParser.PluginInterfaces;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

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

            restClient.Authenticator = new HttpBasicAuthenticator(this.login, this.password);

            var request = new RestRequest("myself");
            var response = await restClient.ExecuteTaskAsync(request);

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK;
        }

        public Task<bool> Login(string token)
        {
            return Task.FromResult(false);
        }

        public async Task<bool> AddJob(JobDefinition job)
        {
            var request = new RestRequest("issue/{issueId}/worklog", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddUrlSegment("issueId", job.TaskId);
            request.AddBody(new WorkLog() { timeSpent = $"{job.Duration}m", comment = job.Description, started = job.Date });

            var response = await restClient.ExecuteTaskAsync(request);

            return response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<TaskHeader> GetTaskHeader(string taskId)
        {
            var request = new RestRequest("issue/{issueId}?fields=summary");
            request.AddUrlSegment("issueId", taskId);

            var response = await restClient.ExecuteTaskAsync(request);

            var title = string.Empty;
            if (response.ResponseStatus == ResponseStatus.Completed && response.StatusCode == HttpStatusCode.OK)
            {
                var content = SimpleJson.DeserializeObject<IssueSummaryResponse>(response.Content);
                title = content.fields["summary"];
            }

            return new TaskHeader() { Title = title };
        }
    }
}
