using System.Collections.Generic;
using System.Threading.Tasks;
using Heavysoft.TimesheetParser.PluginInterfaces;

namespace TimesheetParser.Business.Services
{
    class TaskInfoService
    {
        private readonly Dictionary<string, TaskHeader> taskHeaders = new Dictionary<string, TaskHeader>();
        private readonly ICrm crmClient;
        private readonly object lockObject = new object();

        public TaskInfoService(ICrm crmClient)
        {
            this.crmClient = crmClient;
        }

        public Task<TaskHeader> GetTaskHeader(string taskId)
        {
            return Task.Run(async () =>
            {
                lock (lockObject)
                {
                    if (taskHeaders.ContainsKey(taskId))
                        return taskHeaders[taskId];
                }

                var taskHeader = await crmClient.GetTaskHeader(taskId);
                if (taskHeader != null)
                {
                    lock (lockObject)
                    {
                        if (!taskHeaders.ContainsKey(taskId))
                            taskHeaders[taskId] = taskHeader;
                    }
                }

                return taskHeader;
            });
        }
    }
}
