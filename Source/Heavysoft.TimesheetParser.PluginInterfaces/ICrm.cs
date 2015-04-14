using System.Security;
using System.Threading.Tasks;

namespace Heavysoft.TimesheetParser.PluginInterfaces
{
    internal interface ICrm
    {
        Task<bool> Login(string login, SecureString password);
        Task<bool> Login(string token);
    }
}