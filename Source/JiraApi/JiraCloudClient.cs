using System.Security;
using System.Threading.Tasks;
using Heavysoft.TimesheetParser.PluginInterfaces;

namespace JiraApi
{
    class JiraCloudClient : ICrm
    {
        public Task<bool> Login(string login, SecureString password)
        {
            throw new System.NotImplementedException();
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
