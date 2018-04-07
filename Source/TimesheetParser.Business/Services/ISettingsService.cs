namespace TimesheetParser.Business.Services
{
    public interface ISettingsService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
        void Save();
    }
}