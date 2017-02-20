using GalaSoft.MvvmLight.Command;
using TimesheetParser.Business.Model;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Business.ViewModel
{
    public class JobViewModel : ViewModelBase
    {
        private readonly Job job;
        private bool isDescriptionCopied;
        private bool isDurationCopied;
        private bool isTaskCopied;
        private string taskTitle;
        private readonly IClipboardService clipboardService;

        public JobViewModel(Job job, IClipboardService clipboardService, IDispatchService dispatchService) : base(dispatchService)
        {
            this.job = job;
            this.clipboardService = clipboardService;

            CopyTaskCommand = new RelayCommand(CopyTaskCommand_Executed);
            CopyDurationCommand = new RelayCommand(CopyDurationCommand_Executed);
            CopyDescriptionCommand = new RelayCommand(CopyDescriptionCommand_Executed);
            SkipCommand = new RelayCommand(SkipCommand_Executed);
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

        public bool IsOdd { get; set; }

        public RelayCommand CopyTaskCommand { get; set; }
        public RelayCommand CopyDurationCommand { get; set; }
        public RelayCommand CopyDescriptionCommand { get; set; }

        /// <summary>
        /// Skips job (it'll not be sent to server).
        /// </summary>
        public RelayCommand SkipCommand { get; set; }

        private void CopyTaskCommand_Executed()
        {
            clipboardService.SetText(job.Task);
            IsTaskCopied = true;
        }

        private void CopyDurationCommand_Executed()
        {
            clipboardService.SetText(string.Format("{0:hh}:{0:mm}", job.Duration));
            IsDurationCopied = true;
        }

        private void CopyDescriptionCommand_Executed()
        {
            clipboardService.SetText(job.Description);
            IsDescriptionCopied = true;
        }

        public Job Job => job;

        public string TaskTitle
        {
            get { return taskTitle; }
            set
            {
                taskTitle = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Returns true if job is sent to server or skipped.
        /// </summary>
        public bool IsJobProcessed => job.JobId > 0;

        private void SkipCommand_Executed()
        {
            job.JobId = 1; // @@
            RaisePropertyChanged(nameof(IsJobProcessed));
        }
    }
}