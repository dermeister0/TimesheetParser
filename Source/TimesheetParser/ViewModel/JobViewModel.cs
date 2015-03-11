using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace TimesheetParser.ViewModel
{
    internal class JobViewModel : ViewModelBase
    {
        private readonly Job job;
        private bool isDescriptionCopied;
        private bool isDurationCopied;
        private bool isTaskCopied;

        public JobViewModel(Job job)
        {
            this.job = job;

            CopyTaskCommand = new RelayCommand(CopyTaskCommand_Executed);
            CopyDurationCommand = new RelayCommand(CopyDurationCommand_Executed);
            CopyDescriptionCommand = new RelayCommand(CopyDescriptionCommand_Executed);
        }

        public string Task => "#" + job.Task;
        public string Duration => job.TimeDescription;
        public string Description => job.Description;

        public bool IsTaskCopied
        {
            get { return isTaskCopied; }
            set
            {
                isTaskCopied = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDurationCopied
        {
            get { return isDurationCopied; }
            set
            {
                isDurationCopied = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDescriptionCopied
        {
            get { return isDescriptionCopied; }
            set
            {
                isDescriptionCopied = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand CopyTaskCommand { get; set; }
        public RelayCommand CopyDurationCommand { get; set; }
        public RelayCommand CopyDescriptionCommand { get; set; }

        private void CopyTaskCommand_Executed()
        {
            Clipboard.SetDataObject(job.Task);
            IsTaskCopied = true;
        }

        private void CopyDurationCommand_Executed()
        {
            Clipboard.SetData(DataFormats.UnicodeText, string.Format("{0:hh}:{0:mm}", job.Duration));
            IsDurationCopied = true;
        }

        private void CopyDescriptionCommand_Executed()
        {
            Clipboard.SetData(DataFormats.UnicodeText, job.Description);
            IsDescriptionCopied = true;
        }
    }
}