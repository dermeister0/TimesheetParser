using System.Threading.Tasks;
using TimesheetParser.ViewModel;

namespace TimesheetParser
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var mainVM = ViewModelLocator.Current.MainVM;
            mainVM.LoadPlugins();

            Task.Run(() => mainVM.CheckConnection());

            DataContext = mainVM;
        }
    }
}