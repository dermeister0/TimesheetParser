using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight;
using Heavysoft.TimesheetParser.PluginInterfaces;

namespace TimesheetParser.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private ICrm crmClient;
        private IEnumerable<JobViewModel> jobs;

        public MainViewModel()
        {
            Title = "Timesheet Parser " + Assembly.GetEntryAssembly().GetName().Version;
        }

        public IEnumerable<JobViewModel> Jobs
        {
            get { return jobs; }
            set
            {
                jobs = value;
                RaisePropertyChanged();
            }
        }

        public string CrmPlugin { get; set; } = "None";
        public string Title { get; set; }

        public void LoadPlugins()
        {
            var pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            if (!Directory.Exists(pluginsDir))
            {
                Directory.CreateDirectory(pluginsDir);
            }

            foreach (var file in Directory.GetFiles(pluginsDir))
            {
                var assembly = Assembly.LoadFile(file);
                var type = assembly.GetExportedTypes().FirstOrDefault(t => typeof (ICrm).IsAssignableFrom(t) && t.IsClass);

                if (type != null)
                {
                    crmClient = Activator.CreateInstance(type) as ICrm;

                    CrmPlugin = Path.GetFileName(file);
                    break;
                }
            }
        }
    }
}