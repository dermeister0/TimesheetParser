using System.Windows;

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
            parser.Parse(SourceTextBox.Text);
        }
    }
}