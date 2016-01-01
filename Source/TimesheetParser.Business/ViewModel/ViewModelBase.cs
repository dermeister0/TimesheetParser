using System.Runtime.CompilerServices;
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

        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            DispatchService.InvokeOnUIThread(() => base.RaisePropertyChanged(propertyName));
        }

        protected void RaiseCanExecuteChanged(ICommand command)
        {
            var relayCommand = command as RelayCommand;
            DispatchService.InvokeOnUIThread(() => relayCommand?.RaiseCanExecuteChanged());
        }
    }
}