using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace TimesheetParser.Business.ViewModel
{
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        protected void RaiseCanExecuteChanged(ICommand command)
        {
            var relayCommand = command as RelayCommand;
            relayCommand?.RaiseCanExecuteChanged();
        }
    }
}