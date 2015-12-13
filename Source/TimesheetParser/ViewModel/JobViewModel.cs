using System;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TimesheetParser.Business.Model;

namespace TimesheetParser.ViewModel
{
    internal class JobViewModel : ViewModelBase
    {
        private readonly Job job;
        private bool isDescriptionCopied;
        private bool isDurationCopied;
        private bool isTaskCopied;
        private string taskTitle;

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

        public bool IsOdd { get; set; }

        public RelayCommand CopyTaskCommand { get; set; }
        public RelayCommand CopyDurationCommand { get; set; }
        public RelayCommand CopyDescriptionCommand { get; set; }

        private void CopyTaskCommand_Executed()
        {
            SetClipboardText(job.Task);
            IsTaskCopied = true;
        }

        private void CopyDurationCommand_Executed()
        {
            SetClipboardText(string.Format("{0:hh}:{0:mm}", job.Duration));
            IsDurationCopied = true;
        }

        private void CopyDescriptionCommand_Executed()
        {
            SetClipboardText(job.Description);
            IsDescriptionCopied = true;
        }

        void SetClipboardText(string text)
        {
            try
            {
                Clipboard.SetText(text, TextDataFormat.UnicodeText);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to set clipboard text.");
            }            
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
    }
}