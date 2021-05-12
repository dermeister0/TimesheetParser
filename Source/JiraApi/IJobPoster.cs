using Heavysoft.TimesheetParser.PluginInterfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace JiraApi
{
    interface IJobPoster
    {
        Task<bool> SendAsync(JobDefinition job, HttpClient client);
    }
}
