using System.Collections.Generic;
using Heavysoft.TimesheetParser.PluginInterfaces;
using JiraApi;
using TimesheetParser.Business.Services;
using TimesheetParser.Business.ViewModel;

namespace TimesheetParser.Win10.Services
{
    internal class PluginService : IPluginService
    {
        private readonly IDispatchService dispatchService;

        public PluginService(IDispatchService dispatchService)
        {
            this.dispatchService = dispatchService;
        }

        public IReadOnlyCollection<CrmPluginViewModel> LoadPlugins()
        {
            var plugins = new List<CrmPluginViewModel>();

            ICrm crmClient;
            crmClient = new JiraCloudClient();

            var passwordService = new PasswordService();
            var crmVM = new CrmPluginViewModel(crmClient, passwordService, dispatchService);
            plugins.Add(crmVM);

            return plugins;
        }
    }
}
