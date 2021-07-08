﻿using GalaSoft.MvvmLight.Command;
using TimesheetParser.Business.Model;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Business.ViewModel
{
    public enum JobStatus { New, Success, Failure }

    public class JobViewModel : ViewModelBase
    {
        private readonly Job job;
        private bool isDescriptionCopied;
        private bool isDurationCopied;
        private bool isTaskCopied;
        private string taskTitle;
        private readonly IClipboardService clipboardService;
        private readonly ISettingsService settingsService;
        private JobStatus status;

        public JobViewModel(Job job, IClipboardService clipboardService, ISettingsService settingsService)
        {
            this.job = job;
            this.clipboardService = clipboardService;
            this.settingsService = settingsService;

            jobId = job.JobId;

            CopyTaskCommand = new RelayCommand(CopyTaskCommand_Executed);
            CopyDurationCommand = new RelayCommand(CopyDurationCommand_Executed);
            CopyDescriptionCommand = new RelayCommand(CopyDescriptionCommand_Executed);
            SkipCommand = new RelayCommand(SkipCommand_Executed);
        }

        public string Task => "#" + job.Task;
        public string Duration => job.GetTimeDescription(this.settingsService.Get<DurationFormat>("DurationFormat"));
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

        private int jobId;

        /// <summary>
        /// Id of created job.
        /// </summary>
        public int JobId
        {
            get { return jobId; }
            set
            {
                jobId = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsJobProcessed));
            }
        }

        /// <summary>
        /// Job status.
        /// </summary>
        public JobStatus Status
        {
            get => status;
            set
            {
                status = value;
                RaisePropertyChanged();
            }
        }

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
        public bool IsJobProcessed => JobId > 0;

        private void SkipCommand_Executed()
        {
            JobId = 1; // @@
        }
    }
}