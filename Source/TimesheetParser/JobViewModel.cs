using System.Windows;
using Microsoft.Practices.Prism.Commands;

namespace TimesheetParser
{
    internal class JobViewModel
    {
        private readonly Job job;

        public JobViewModel(Job job)
        {
            this.job = job;

            CopyTaskCommand = new DelegateCommand(CopyTaskCommand_Executed);
            CopyDurationCommand = new DelegateCommand(CopyDurationCommand_Executed);
            CopyDescriptionCommand = new DelegateCommand(CopyDescriptionCommand_Executed);
        }

        public string Duration => job.TimeDescription;
        public bool IsTaskCopied { get; set; }
        public bool IsDescriptionCopied { get; set; }
        public bool IsDurationCopied { get; set; }
        public DelegateCommand CopyTaskCommand { get; set; }
        public DelegateCommand CopyDurationCommand { get; set; }
        public DelegateCommand CopyDescriptionCommand { get; set; }

        private void CopyTaskCommand_Executed()
        {
            Clipboard.SetData(DataFormats.UnicodeText, job.Task);
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