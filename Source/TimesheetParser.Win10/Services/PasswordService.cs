using System;
using System.Linq;
using Windows.Security.Credentials;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Win10.Services
{
    internal class PasswordService : IPasswordService
    {
        private readonly string pluginName;
        private readonly PasswordVault vault = new PasswordVault();

        public PasswordService(string pluginName)
        {
            this.pluginName = pluginName;
        }

        public string Login { get; set; }
        public string Password { get; set; }

        public void LoadCredential()
        {
            try
            {
                var credential = vault.FindAllByResource(GetResource()).FirstOrDefault();
                if (credential != null)
                {
                    Login = credential.UserName;
                    Password = credential.Password;
                }
            }
            catch (Exception)
            {
                // No passwords are saved for this resource.
            }
        }

        public void SaveCredential()
        {
            vault.Add(new PasswordCredential(GetResource(), Login, Password));
        }

        public void DeleteCredential()
        {
            vault.Remove(vault.Retrieve(GetResource(), Login));
        }

        private string GetResource()
        {
            return $"TimesheetParser/{pluginName}";
        }
    }
}