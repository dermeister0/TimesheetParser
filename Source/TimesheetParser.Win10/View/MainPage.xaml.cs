using Windows.UI.Xaml.Controls;
using TimesheetParser.Business.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TimesheetParser.Win10.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var mainVM = DataContext as MainViewModel;
            mainVM?.Initialize();
        }
    }
}
