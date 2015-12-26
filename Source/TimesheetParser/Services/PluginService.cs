using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Heavysoft.TimesheetParser.PluginInterfaces;
using TimesheetParser.Business.Services;
using TimesheetParser.Business.ViewModel;

namespace TimesheetParser.Services
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
            var pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            if (!Directory.Exists(pluginsDir))
            {
                Directory.CreateDirectory(pluginsDir);
            }

            var plugins = new List<CrmPluginViewModel>();

            foreach (var file in Directory.GetFiles(pluginsDir, "*.dll"))
            {
                var assembly = Assembly.LoadFile(file);
                var type = assembly.GetExportedTypes().FirstOrDefault(t => typeof(ICrm).IsAssignableFrom(t) && t.IsClass);

                if (type == null)
                    continue;

                var crmClient = Activator.CreateInstance(type) as ICrm;
                if (crmClient == null)
                    continue;

                var passwordService = new PasswordService(crmClient.GetName());
                var crmVM = new CrmPluginViewModel(crmClient, passwordService, dispatchService);
                plugins.Add(crmVM);
            }

            return plugins;
        }
    }
}
