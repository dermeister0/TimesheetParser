using CredentialManagement;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Services
{
    internal class PasswordService : IPasswordService
    {
        private readonly string pluginName;

        public string Login { get; set; }
        public string Password { get; set; }

        public PasswordService(string pluginName)
        {
            this.pluginName = pluginName;
        }

        public void LoadCredential()
        {
            var credential = new Credential() { Target = GetTarget() };
            credential.Load();

            Login = credential.Username;
            Password = credential.Password;
        }

        public void SaveCredential()
        {
            var credential = new Credential()
            {
                Target = GetTarget(),
                PersistanceType = PersistanceType.LocalComputer,
                Username = Login,
                Password = Password
            };
            credential.Save();
        }

        public void DeleteCredential()
        {
            var credential = new Credential() { Target = GetTarget() };
            credential.Delete();
        }

        private string GetTarget()
        {
            return $"TimesheetParser/{pluginName}";
        }
    }
}
