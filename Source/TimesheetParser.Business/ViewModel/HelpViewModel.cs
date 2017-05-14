using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Business.ViewModel
{
    /// <summary>
    /// View-model for Help page.
    /// </summary>
    public class HelpViewModel : ViewModelBase
    {
        private readonly IPortableNavigationService navigationService;

        public HelpViewModel(IPortableNavigationService navigationService)
        {
            this.navigationService = navigationService;

            BackCommand = new RelayCommand(BackCommand_Executed);
        }

        /// <summary>
        /// Command for Back button.
        /// </summary>
        public ICommand BackCommand { get; set; }

        private void BackCommand_Executed()
        {
            navigationService.NavigateTo(Location.Main);
        }
    }
}
