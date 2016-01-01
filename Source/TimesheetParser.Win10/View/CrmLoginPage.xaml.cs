using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TimesheetParser.Business.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TimesheetParser.Win10.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CrmLoginPage : Page
    {
        public CrmLoginPage()
        {
            this.InitializeComponent();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var crmLoginVM = DataContext as CrmLoginViewModel;
            if (crmLoginVM != null)
            {
                crmLoginVM.Password = ((PasswordBox)sender).Password;
            }
        }

        private void CrmLoginPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            var crmLoginVM = DataContext as CrmLoginViewModel;
            if (crmLoginVM?.Password != null)
            {
                PasswordBox.Password = crmLoginVM.Password;
            }
        }
    }
}
