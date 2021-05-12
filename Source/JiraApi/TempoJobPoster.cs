using Heavysoft.TimesheetParser.PluginInterfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JiraApi
{
    class TempoJobPoster : IJobPoster
    {
        public Task<bool> SendAsync(JobDefinition job, HttpClient client)
        {
            throw new NotImplementedException();
        }
    }
}
