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
        public ICrm Client => crmClient;

        public CrmPluginViewModel(ICrm crmClient)
        {
            this.crmClient = crmClient;
            Name = crmClient.GetName();

            passwordHelper = new PasswordHelper(ViewModelLocator.Current.CrmLoginVM, Name);
            passwordHelper.LoadCredential();
        }
    }
}
