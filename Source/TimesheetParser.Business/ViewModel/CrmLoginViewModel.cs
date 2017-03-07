using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Business.ViewModel
{
    public class CrmLoginViewModel : ViewModelBase
    {
        private string login;
        private string password;
        private readonly IPortableNavigationService navigationService;

        public CrmLoginViewModel(IPortableNavigationService navigationService)
        {
            this.navigationService = navigationService;

            LoginCommand = new RelayCommand(LoginCommand_Executed, () => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrEmpty(Password));
        }

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                RaisePropertyChanged();
                RaiseCanExecuteChanged(LoginCommand);
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged();
                RaiseCanExecuteChanged(LoginCommand);
            }
        }

        public ICommand LoginCommand { get; set; }
        public CrmPluginViewModel SourcePlugin { private get; set; }

        private void LoginCommand_Executed()
        {
            SourcePlugin?.CheckConnection(Login, Password);

            navigationService.NavigateTo(Location.Main);
        }
    }
}