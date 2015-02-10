using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace TimesheetParser
{
    class MainViewModel
    {
        public ICommand CopyCommand { get; set; }

        public MainViewModel()
        {
            CopyCommand = new DelegateCommand<string>(CopyCommand_Executed);
        }

        void CopyCommand_Executed(string param)
        {
            
        }
    }
}
