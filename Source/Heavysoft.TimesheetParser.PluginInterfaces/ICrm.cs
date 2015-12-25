using System.Threading.Tasks;

namespace Heavysoft.TimesheetParser.PluginInterfaces
{
    public interface ICrm
    {
        string GetName();
        bool IsValidTask(string taskId);

        Task<bool> Login(string login, string password);
        Task<bool> Login(string token);
        Task<bool> AddJob(JobDefinition job);
        Task<TaskHeader> GetTaskHeader(string taskId);
    }
}