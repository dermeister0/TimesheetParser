using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Heavysoft.TimesheetParser.PluginInterfaces;
using Microsoft.Practices.ServiceLocation;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Business.ViewModel
{
    public class CrmPluginViewModel : ViewModelBase
    {
        private readonly IPasswordService passwordService;
        private readonly ICrm crmClient;
        private readonly TaskInfoService taskInfoService;
        private bool isBusy;
        private bool isConnected;
        private readonly IDispatchService dispatchService;

        public CrmPluginViewModel(ICrm crmClient, IPasswordService passwordService, IDispatchService dispatchService)
        {
            this.crmClient = crmClient;
            this.dispatchService = dispatchService;
            this.passwordService = passwordService;
            taskInfoService = new TaskInfoService(crmClient);

            Name = crmClient.GetName();

            LoginCommand = new RelayCommand(LoginCommand_Executed, () => !isBusy);
        }

        public bool IsConnected
        {
            get { return isConnected; }
            private set
            {
                isConnected = value;
                RaisePropertyChanged();
            }
        }

        protected bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged();
                dispatchService.InvokeOnUIThread(() => (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged());
            }
        }

        public string Name { get; }
        public ICrm Client => crmClient;

        public ICommand LoginCommand { get; }

        public Task<TaskHeader> GetTaskHeader(string taskId)
        {
            return taskInfoService.GetTaskHeader(taskId);
        }

        private async Task DoLogin()
        {
            if (!string.IsNullOrEmpty(passwordService.Login) && !string.IsNullOrEmpty(passwordService.Password))
            {
                IsBusy = true;
                try
                {
                    IsConnected = await crmClient.Login(passwordService.Login, passwordService.Password);

                    if (!isConnected)
                    {
                        passwordService.DeleteCredential();
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async void CheckConnection()
        {
            passwordService.LoadCredential();
            await DoLogin();
        }

        public async void CheckConnection(string login, string password)
        {
            passwordService.Login = login;
            passwordService.Password = password;
            await DoLogin();

            if (isConnected)
            {
                passwordService.SaveCredential();
            }
        }

        private void LoginCommand_Executed()
        {
            var crmLoginVM = ViewModelLocator.Current.CrmLoginVM;
            crmLoginVM.Login = passwordService.Login;
            crmLoginVM.Password = passwordService.Password;
            crmLoginVM.SourcePlugin = this;

            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.NavigateTo("/View/CrmLoginPage.xaml");
        }
    }
}
