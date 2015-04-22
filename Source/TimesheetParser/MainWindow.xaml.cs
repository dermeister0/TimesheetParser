using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TimesheetParser.ViewModel;

namespace TimesheetParser
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainVM;

        public MainWindow()
        {
            InitializeComponent();

            mainVM = new MainViewModel();
            mainVM.LoadPlugins();

            Task.Run(() => mainVM.CheckConnection());

            DataContext = mainVM;
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var parser = new Parser();
            var result = parser.Parse(SourceTextBox.Text, DistributeIdleCheckBox.IsChecked == true);
            DestinationTextBox.Text = result.Format();

            mainVM.Jobs = result.Jobs.Where(j => !string.IsNullOrEmpty(j.Task)).Select(j => new JobViewModel(j)).ToList();

            string previousTask = null;
            bool isOdd = false;

            foreach (var jobVM in mainVM.Jobs)
            {
                if (jobVM.Task != previousTask)
                {
                    isOdd = !isOdd;
                    previousTask = jobVM.Task;
                }

                jobVM.IsOdd = isOdd;
            }
        }
    }
}