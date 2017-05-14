namespace TimesheetParser.Business.Services
{
    public enum Location
    {
        Main,
        Login,
        Help
    }

    public interface IPortableNavigationService
    {
        void NavigateTo(Location pageKey);
    }
}
