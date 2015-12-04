using CredentialManagement;
using TimesheetParser.ViewModel;

namespace TimesheetParser.Support
{
    internal class PasswordHelper
    {
        private readonly string pluginName;
        private readonly CrmLoginViewModel crmLoginVM;

        public PasswordHelper(CrmLoginViewModel crmLoginVM, string pluginName)
        {
            this.crmLoginVM = crmLoginVM;
            this.pluginName = pluginName;
        }

        public void LoadCredential()
        {
            var credential = new Credential() { Target = GetTarget() };
            credential.Load();

            crmLoginVM.Login = credential.Username;
            crmLoginVM.Password = credential.SecurePassword;
        }

        public void SaveCredential()
        {
            var credential = new Credential()
            {
                Target = GetTarget(),
                PersistanceType = PersistanceType.LocalComputer,
                Username = crmLoginVM.Login,
                SecurePassword = crmLoginVM.Password
            };
            credential.Save();
        }

        private string GetTarget()
        {
            return $"TimesheetParser/{pluginName}";
        }
    }
}
