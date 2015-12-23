using Heavysoft.TimesheetParser.PluginInterfaces;
using TimesheetParser.Support;

namespace TimesheetParser.ViewModel
{
    public class CrmPluginViewModel
    {
        private readonly PasswordHelper passwordHelper;
        private readonly ICrm crmClient;

        public bool IsConnected { get; set; }
        public string Name { get; }

        public CrmPluginViewModel(ICrm crmClient, string name)
        {
            this.crmClient = crmClient;
            Name = name;

            passwordHelper = new PasswordHelper(ViewModelLocator.Current.CrmLoginVM, name);
            passwordHelper.LoadCredential();
        }
    }
}
