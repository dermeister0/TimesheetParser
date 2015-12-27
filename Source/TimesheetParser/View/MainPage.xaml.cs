using System.Threading.Tasks;
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
            mainVM?.LoadPlugins();

            Task.Run(() => mainVM?.CheckConnection());
        }
    }
}