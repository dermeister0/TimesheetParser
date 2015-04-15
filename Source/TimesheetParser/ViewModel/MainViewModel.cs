using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Heavysoft.TimesheetParser.PluginInterfaces;

namespace TimesheetParser.ViewModel
{
    internal class MainViewModel
    {
        private ICrm crmClient;

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

        public string CrmPlugin { get; set; } = "None";
    }
}