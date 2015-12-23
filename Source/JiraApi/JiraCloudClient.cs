using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Heavysoft.TimesheetParser.PluginInterfaces;
using RestSharp;
using RestSharp.Authenticators;

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

        public Task<bool> AddJob(JobDefinition job)
        {
            var request = new RestRequest("issue/{issueId}/worklog");
            request.AddUrlSegment("issueId", job.TaskId);

            throw new System.NotImplementedException();
        }

        public Task<TaskHeader> GetTaskHeader(string taskId)
        {
            throw new System.NotImplementedException();
        }
    }
}
