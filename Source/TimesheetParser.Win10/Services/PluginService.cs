using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;
using Heavysoft.TimesheetParser.PluginInterfaces;
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

            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            List<Assembly> assemblies = new List<Assembly>();
            foreach (StorageFile file in folder.GetFilesAsync().AsTask().Result)
            {
                if (file.FileType == ".dll" && file.Name.Contains("Api"))
                {
                    var fileName = file.Name.Substring(0, file.Name.Length - file.FileType.Length);
                    Assembly assembly = Assembly.Load(new AssemblyName() { Name = fileName });
                    var type = assembly.GetExportedTypes().FirstOrDefault(t => typeof(ICrm).IsAssignableFrom(t));

                    if (type == null)
                        continue;

                    var crmClient = Activator.CreateInstance(type) as ICrm;
                    if (crmClient == null)
                        continue;

                    var passwordService = new PasswordService();
                    var crmVM = new CrmPluginViewModel(crmClient, passwordService, dispatchService);
                    plugins.Add(crmVM);
                }
            }

            return plugins;
        }
    }
}
