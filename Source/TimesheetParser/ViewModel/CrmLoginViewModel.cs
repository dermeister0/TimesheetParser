using System.Security;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using TimesheetParser.Messages;

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

        private void LoginCommand_Executed()
        {
            Messenger.Default.Send(new LoginMessage { Login = Login, Password = Password });

            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.NavigateTo("MainPage.xaml");
        }
    }
}