using System.Linq;
using System.Windows;
using TimesheetParser.Extensions;
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

            DataContext = mainVM;
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var parser = new Parser();
            var result = parser.Parse(SourceTextBox.Text, DistributeIdleCheckBox.IsChecked == true);
            DestinationTextBox.Text = result.Format();

            mainVM.Jobs = result.Jobs.Where(j => !string.IsNullOrEmpty(j.Task)).Select(j => new JobViewModel(j)).ToList();

            foreach (var jobVM in mainVM.Jobs.SelectOdds())
            {
                jobVM.IsOdd = true;
            }
        }
    }
}