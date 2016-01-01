using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Business.ViewModel
{
    public class CrmLoginViewModel : ViewModelBase
    {
        private string login;
        private string password;
        private readonly IPortableNavigationService navigationService;
        private readonly IDispatchService dispatchService;

        public CrmLoginViewModel(IPortableNavigationService navigationService, IDispatchService dispatchService)
        {
            this.navigationService = navigationService;
            this.dispatchService = dispatchService;

            LoginCommand = new RelayCommand(LoginCommand_Executed, () => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrEmpty(Password));
        }

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                RaisePropertyChanged();
                dispatchService.InvokeOnUIThread(() => (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged());
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged();
                dispatchService.InvokeOnUIThread(() => (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged());
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