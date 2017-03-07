using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if DEBUG

using Windows.Storage;

#endif

using Heavysoft.TimesheetParser.PluginInterfaces;
using TimesheetParser.Business.Services;
using TimesheetParser.Business.ViewModel;

namespace TimesheetParser.Win10.Services
{
    internal class PluginService : IPluginService
    {
        private readonly IPortableNavigationService navigationService;

        public PluginService(IPortableNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public IReadOnlyCollection<CrmPluginViewModel> LoadPlugins()
        {
            var plugins = new List<CrmPluginViewModel>();
            // ReSharper disable once JoinDeclarationAndInitializer
            IEnumerable<string> assemblyNames;

#if DEBUG
            assemblyNames = Windows.ApplicationModel.Package.Current.InstalledLocation.GetFilesAsync().AsTask().Result
                .Where(file => file.FileType == ".dll" && file.Name.Contains("Api"))
                .Select(file => file.Name.Substring(0, file.Name.Length - file.FileType.Length));
#else
            assemblyNames = new[] { "JiraApi", "SaritasaApi" };
#endif

            foreach (var name in assemblyNames)
            {
                var assembly = Assembly.Load(new AssemblyName() { Name = name });
                var type = assembly.GetExportedTypes().FirstOrDefault(t => typeof(ICrm).IsAssignableFrom(t));

                if (type == null)
                    continue;

                var crmClient = Activator.CreateInstance(type) as ICrm;
                if (crmClient == null)
                    continue;

                var passwordService = new PasswordService(crmClient.GetName());
                var crmVM = new CrmPluginViewModel(crmClient, passwordService, navigationService);
                plugins.Add(crmVM);
            }

            return plugins;
        }
    }
}
