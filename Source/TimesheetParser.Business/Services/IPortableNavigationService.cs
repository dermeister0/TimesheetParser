namespace TimesheetParser.Business.Services
{
    public enum Location
    {
        Main,
        Login
    }

    public interface IPortableNavigationService
    {
        void NavigateTo(Location pageKey);
    }
}
