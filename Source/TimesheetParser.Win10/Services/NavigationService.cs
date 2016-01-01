using TimesheetParser.Business.Services;

namespace TimesheetParser.Win10.Services
{
    internal class NavigationService : IPortableNavigationService
    {
        private readonly GalaSoft.MvvmLight.Views.NavigationService navigationService;

        public NavigationService()
        {
            navigationService = new GalaSoft.MvvmLight.Views.NavigationService();
            navigationService.Configure(Location.Main.ToString(), typeof(View.MainPage));
            navigationService.Configure(Location.Login.ToString(), typeof(View.CrmLoginPage));
        }

        public void NavigateTo(Location pageKey)
        {
            navigationService.NavigateTo(pageKey.ToString());
        }
    }
}
