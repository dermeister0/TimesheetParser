using System.Collections.Generic;
using TimesheetParser.Business.ViewModel;

namespace TimesheetParser.Business.Services
{
    public interface IPluginService
    {
        IReadOnlyCollection<CrmPluginViewModel> LoadPlugins();
    }
}
