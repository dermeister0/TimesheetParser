using System.Security;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace TimesheetParser.ViewModel
{
    internal class CrmLoginViewModel
    {
        public CrmLoginViewModel()
        {
            LoginCommand = new RelayCommand(LoginCommand_Executed, () => !string.IsNullOrWhiteSpace(Login) && Password != null && Password.Length > 0);
        }

        public string Login { get; set; }
        public SecureString Password { get; set; }
        public ICommand LoginCommand { get; set; }
        public CrmPluginViewModel SourcePlugin { get; set; }

        private void LoginCommand_Executed()
        {
            if (SourcePlugin != null)
            {
                SourcePlugin.CheckConnection(Login, Password);
            }

            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.NavigateTo("MainPage.xaml");
        }
    }
}