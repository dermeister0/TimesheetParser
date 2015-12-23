using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using Heavysoft.TimesheetParser.PluginInterfaces;
using Microsoft.Practices.ServiceLocation;
using TimesheetParser.Services;
using TimesheetParser.Support;

namespace TimesheetParser.ViewModel
{
    public class CrmPluginViewModel : ViewModelBase
    {
        private readonly PasswordHelper passwordHelper;
        private readonly ICrm crmClient;
        private readonly TaskInfoService taskInfoService;
        private bool isBusy;
        private bool isConnected;

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                RaisePropertyChanged();
            }
        }

        public string Name { get; }
        public ICrm Client => crmClient;

        public ICommand LoginCommand { get; }

        public CrmPluginViewModel(ICrm crmClient)
        {
            this.crmClient = crmClient;
            Name = crmClient.GetName();

            taskInfoService = new TaskInfoService(crmClient);

            passwordHelper = new PasswordHelper(Name);

            LoginCommand = new RelayCommand(LoginCommand_Executed, () => !isBusy);
        }

        public Task<TaskHeader> GetTaskHeader(string taskId)
        {
            return taskInfoService.GetTaskHeader(taskId);
        }

        private async Task DoLogin()
        {
            var password = passwordHelper.Password.ConvertToUnsecureString();
            if (!string.IsNullOrEmpty(passwordHelper.Login) && !string.IsNullOrEmpty(password))
            {
                isBusy = true;
                try
                {
                    IsConnected = await crmClient.Login(passwordHelper.Login, password);

                    if (!isConnected)
                    {
                        passwordHelper.DeleteCredential();
                    }
                }
                finally
                {
                    isBusy = false;
                }
            }
        }

        public async void CheckConnection()
        {
            passwordHelper.LoadCredential();
            await DoLogin();
        }

        public async void CheckConnection(string login, SecureString password)
        {
            passwordHelper.Login = login;
            passwordHelper.Password = password;
            await DoLogin();

            if (isConnected)
            {
                passwordHelper.SaveCredential();
            }
        }

        private void LoginCommand_Executed()
        {
            var crmLoginVM = ViewModelLocator.Current.CrmLoginVM;
            crmLoginVM.Login = passwordHelper.Login;
            crmLoginVM.Password = passwordHelper.Password;
            crmLoginVM.SourcePlugin = this;

            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.NavigateTo("CrmLoginPage.xaml");
        }
    }
}
