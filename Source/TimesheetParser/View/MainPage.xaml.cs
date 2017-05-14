using GalaSoft.MvvmLight.Ioc;
using System;
using System.Windows.Controls;
using TimesheetParser.Business.ViewModel;
using TimesheetParser.Services;

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

            var updateService = SimpleIoc.Default.GetInstance<UpdateService>();
            updateService.UpdateApp();
        }

        private void TimestampButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            JobsTextBox.SelectedText = DateTime.Now.ToShortTimeString();
            JobsTextBox.CaretIndex += JobsTextBox.SelectedText.Length;
            JobsTextBox.SelectionLength = 0;
        }
    }
}