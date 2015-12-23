using System.Net;
using System.Threading.Tasks;
using Heavysoft.TimesheetParser.PluginInterfaces;
using RestSharp;
using RestSharp.Authenticators;

namespace JiraApi
{
    internal class ApiResult
    {
        public string Message { get; set; }
        public string StatusCode { get; set; }
    }

    public class JiraCloudClient : ICrm
    {
        private const string ApiUrl = "https://saritasa.atlassian.net/rest/api/2/";

        private string login;
        private string password;
        private RestClient restClient;

        public JiraCloudClient()
        {
            restClient = new RestClient(ApiUrl);
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
            throw new System.NotImplementedException();
        }

        public Task<TaskHeader> GetTaskHeader(string taskId)
        {
            throw new System.NotImplementedException();
        }
    }
}
