using System.Linq;
using System.Windows;
using TimesheetParser.ViewModel;

namespace TimesheetParser
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var parser = new Parser();
            var result = parser.Parse(SourceTextBox.Text, DistributeIdleCheckBox.IsChecked == true);
            DestinationTextBox.Text = result.Format();

            JobsListView.ItemsSource = result.Jobs.Where(j => !string.IsNullOrEmpty(j.Task)).Select(j => new JobViewModel(j));
        }
    }
}