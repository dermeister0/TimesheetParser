using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Business.ViewModel
{
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        public IDispatchService DispatchService { get; }

        public ViewModelBase(IDispatchService dispatchService)
        {
            DispatchService = dispatchService;
        }

        protected void RaiseCanExecuteChanged(ICommand command)
        {
            var relayCommand = command as RelayCommand;
            relayCommand?.RaiseCanExecuteChanged();
        }
    }
}