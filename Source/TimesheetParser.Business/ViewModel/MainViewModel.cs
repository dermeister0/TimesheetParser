using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Heavysoft.TimesheetParser.PluginInterfaces;
using TimesheetParser.Business.Services;

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
        private readonly IPluginService pluginService;

        public MainViewModel(IPluginService pluginService)
        {
            this.pluginService = pluginService;

            //@@Title = "Timesheet Parser " + Assembly.GetEntryAssembly().GetName().Version;
            Title = "Timesheet Parser";
            JobsDate = DateTime.Now;

            GenerateCommand = new RelayCommand(GenerateCommand_Executed);
            SubmitJobsCommand = new RelayCommand(SubmitJobs_Executed);
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

        public string Title { get; set; }

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                RaisePropertyChanged();
            }
        }

        public ICommand GenerateCommand { get; set; }
        public ICommand SubmitJobsCommand { get; set; }

        public DateTime JobsDate
        {
            get { return jobsDate; }
            set
            {
                jobsDate = value;
                RaisePropertyChanged();
            }
        }

        public IReadOnlyCollection<CrmPluginViewModel> CrmPlugins { get; set; }

        private void CrmVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CrmPluginViewModel.IsConnected))
            {
                var crmVM = sender as CrmPluginViewModel;
                if (crmVM != null)
                {
                    IsConnected |= crmVM.IsConnected;
                }
            }
        }

        public void CheckConnection()
        {
            foreach (var pluginVM in CrmPlugins)
            {
                pluginVM.CheckConnection();
            }
        }

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

        private async void SubmitJobs_Executed()
        {
            foreach (var jobVM in Jobs)
            {
                // Job is submitted already.
                if (jobVM.Job.JobId != 0)
                    continue;

                foreach (var pluginVM in CrmPlugins.Where(p => p.IsConnected))
                {
                    if (!pluginVM.Client.IsValidTask(jobVM.Job.Task))
                        continue;

                    var taskHeader = await pluginVM.GetTaskHeader(jobVM.Job.Task);
                    jobVM.TaskTitle = taskHeader.Title;

                    jobVM.IsTaskCopied = true;
                    jobVM.IsDescriptionCopied = true;
                    jobVM.IsDurationCopied = true;

                    await pluginVM.Client.AddJob(new JobDefinition
                    {
                        TaskId = jobVM.Job.Task,
                        Date = JobsDate,
                        Description = jobVM.Description,
                        Duration = (int)jobVM.Job.Duration.TotalMinutes,
                        IsBillable = taskHeader.IsBillable,
                    });
                    jobVM.Job.JobId = 1; // @@

                    break;
                }
            }
        }

        public void LoadPlugins()
        {
            CrmPlugins = pluginService.LoadPlugins();
        }
    }
}