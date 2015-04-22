using System.Windows;
using System.Windows.Controls;
using TimesheetParser.ViewModel;

namespace TimesheetParser
{
    /// <summary>
    /// Interaction logic for CrmLoginPage.xaml
    /// </summary>
    public partial class CrmLoginPage : Page
    {
        public CrmLoginPage()
        {
            InitializeComponent();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var crmLoginVM = DataContext as CrmLoginViewModel;
            if (crmLoginVM != null)
            {
                crmLoginVM.Password = ((PasswordBox) e.Source).SecurePassword;
            }
        }
    }
}