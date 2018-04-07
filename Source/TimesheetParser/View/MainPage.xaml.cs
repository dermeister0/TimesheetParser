using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Windows.Controls;
using System.Windows.Input;
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

            InsertCurrentTimestampCommand = new RelayCommand(InsertCurrentTimestampCommand_Executed);
            InsertIdleCommand = new RelayCommand(InsertIdleCommand_Executed);
        }

        /// <summary>
        /// Command for Insert Current Timestamp button.
        /// </summary>
        public ICommand InsertCurrentTimestampCommand { get; }

        /// <summary>
        /// Command for Insert 'Idle.' And Current Timestamp button.
        /// </summary>
        public ICommand InsertIdleCommand { get; }

        private void InsertCurrentTimestampCommand_Executed()
        {
            JobsTextBox.SelectedText = DateTime.Now.ToShortTimeString() + Environment.NewLine;
            JobsTextBox.CaretIndex += JobsTextBox.SelectedText.Length;
            JobsTextBox.SelectionLength = 0;
        }

        private void InsertIdleCommand_Executed()
        {
            JobsTextBox.SelectedText = "Idle." + Environment.NewLine + 
                DateTime.Now.ToShortTimeString() + Environment.NewLine;
            JobsTextBox.CaretIndex += JobsTextBox.SelectedText.Length;
            JobsTextBox.SelectionLength = 0;
        }
    }
}