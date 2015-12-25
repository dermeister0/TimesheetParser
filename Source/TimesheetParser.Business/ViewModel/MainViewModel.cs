using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TimesheetParser.Business.Model;

namespace TimesheetParser.Business.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEnumerable<JobViewModel> jobs;
        private bool isConnected;
        private string sourceText;
        private string resultText;
        private bool distributeIdle;
        private DateTime jobsDate;

        public MainViewModel()
        {
            GenerateCommand = new RelayCommand(GenerateCommand_Executed);

            // Initialize attached property.
            SourceText = string.Empty;

            if (IsInDesignMode)
            {
                Jobs = new List<JobViewModel>()
                {
                    new JobViewModel(new Job() { StartTime = DateTime.Parse("12/1/2015"), Description = "Just a test.", Task = "1" }) { IsOdd = true },
                    new JobViewModel(new Job() { StartTime = DateTime.Parse("12/2/2015"), Description = "Another job.", Task = "2" }),
                    new JobViewModel(new Job() { StartTime = DateTime.Parse("12/3/2015"), Description = "Third job.", Task = "3" }) { IsOdd = true },
                };
            }
        }

        public IEnumerable<JobViewModel> Jobs
        {
            get { return jobs; }
            set
            {
                jobs = value;
                RaisePropertyChanged();
            }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                RaisePropertyChanged();
            }
        }

        public string SourceText
        {
            get { return sourceText; }
            set
            {
                sourceText = value;
                RaisePropertyChanged();
            }
        }

        public string ResultText
        {
            get { return resultText; }
            set
            {
                resultText = value;
                RaisePropertyChanged();
            }
        }

        public bool DistributeIdle
        {
            get { return distributeIdle; }
            set
            {
                distributeIdle = value;
                RaisePropertyChanged();
            }
        }

        public DateTime JobsDate
        {
            get { return jobsDate; }
            set
            {
                jobsDate = value;
                RaisePropertyChanged();
            }
        }

        public ICommand GenerateCommand { get; set; }
        public ICommand SubmitJobsCommand { get; set; }

        private void GenerateCommand_Executed()
        {
            var parser = new Parser();
            var result = parser.Parse(SourceText, DistributeIdle);
            ResultText = result.Format();

            Jobs = result.Jobs.Where(j => !string.IsNullOrEmpty(j.Task)).Select(j => new JobViewModel(j)).ToList();

            string previousTask = null;
            bool isOdd = false;

            foreach (var jobVM in Jobs)
            {
                if (jobVM.Task != previousTask)
                {
                    isOdd = !isOdd;
                    previousTask = jobVM.Task;
                }

                jobVM.IsOdd = isOdd;
            }
        }
    }
}
