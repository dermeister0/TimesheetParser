using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace TimesheetParser.Business.ViewModel
{
    public class CrmLoginViewModel : ViewModelBase
    {
        private string login;
        private string password;

        public CrmLoginViewModel()
        {
            LoginCommand = new RelayCommand(LoginCommand_Executed, () => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrEmpty(Password));
        }

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                RaisePropertyChanged();
                (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged();
                (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand LoginCommand { get; set; }
        public CrmPluginViewModel SourcePlugin { private get; set; }

        private void LoginCommand_Executed()
        {
            SourcePlugin?.CheckConnection(Login, Password);

            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.NavigateTo("MainPage.xaml");
        }
    }
}