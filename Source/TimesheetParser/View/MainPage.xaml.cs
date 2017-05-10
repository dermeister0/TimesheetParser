using Squirrel;
using System.Configuration;
using System.Windows.Controls;
using TimesheetParser.Business.ViewModel;

namespace TimesheetParser.View
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            var mainVM = DataContext as MainViewModel;
            mainVM?.Initialize();

            UpdateApp();
        }

        private async void UpdateApp()
        {
            using (var manager = new UpdateManager(ConfigurationManager.AppSettings["UpdateRoot"]))
            {
                await manager.UpdateApp();
            }
        }
    }
}