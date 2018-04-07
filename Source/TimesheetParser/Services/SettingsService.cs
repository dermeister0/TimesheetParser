using TimesheetParser.Business.Services;
using TimesheetParser.Properties;

namespace TimesheetParser.Services
{
    public class SettingsService : ISettingsService
    {
        public T Get<T>(string key)
        {
            return (T)Settings.Default[key];
        }

        public void Set<T>(string key, T value)
        {
            Settings.Default[key] = value;
        }

        public void Save()
        {
            Settings.Default.Save();
        }
    }
}