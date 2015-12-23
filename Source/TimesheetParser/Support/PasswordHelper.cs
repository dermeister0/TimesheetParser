using System.Security;
using CredentialManagement;

namespace TimesheetParser.Support
{
    internal class PasswordHelper
    {
        private readonly string pluginName;

        public string Login { get; private set; }
        public SecureString Password { get; private set; }

        public PasswordHelper(string pluginName)
        {
            this.pluginName = pluginName;
        }

        public void LoadCredential()
        {
            var credential = new Credential() { Target = GetTarget() };
            credential.Load();

            Login = credential.Username;
            Password = credential.SecurePassword;
        }

        public void SaveCredential()
        {
            var credential = new Credential()
            {
                Target = GetTarget(),
                PersistanceType = PersistanceType.LocalComputer,
                Username = Login,
                SecurePassword = Password
            };
            credential.Save();
        }

        private string GetTarget()
        {
            return $"TimesheetParser/{pluginName}";
        }
    }
}
