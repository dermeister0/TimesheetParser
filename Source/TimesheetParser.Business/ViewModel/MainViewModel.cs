using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private readonly IClipboardService clipboardService;
        private IReadOnlyCollection<CrmPluginViewModel> crmPlugins;
        private bool initialized;

        public MainViewModel(IPluginService pluginService, IClipboardService clipboardService, IDispatchService dispatchService) : base(dispatchService)
        {
            this.pluginService = pluginService;
            this.clipboardService = clipboardService;

            var version = typeof (MainViewModel).GetTypeInfo().Assembly.GetName().Version;
            Title = $"Timesheet Parser {version.Major}.{version.Minor}";
            JobsDate = DateTime.Now;

            GenerateCommand = new RelayCommand(GenerateCommand_Executed);
            SubmitJobsCommand = new RelayCommand(SubmitJobs_Executed);
        }

        public void Initialize()
        {
            if (initialized)
                return;

            initialized = true;
            Task.Run(() =>
                {
                    LoadPlugins();
                    CheckConnection();
                });
        }

        #region Properties

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

        public IReadOnlyCollection<CrmPluginViewModel> CrmPlugins
        {
            get { return crmPlugins; }
            set
            {
                crmPlugins = value;
                RaisePropertyChanged();
            }
        }

        public void CheckConnection()
        {
            foreach (var pluginVM in CrmPlugins)
            {
                pluginVM.CheckConnection();
            }
        }

        #endregion

        #region Commands

        private void GenerateCommand_Executed()
        {
            var parser = new Parser();
            var result = parser.Parse(SourceText, DistributeIdle);
            ResultText = result.Format();

            Jobs = result.Jobs.Where(j => !string.IsNullOrEmpty(j.Task)).Select(j => new JobViewModel(j, clipboardService, DispatchService)).ToList();

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
                if (jobVM.JobId != 0)
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

                    try
                    {
                        await pluginVM.Client.AddJob(new JobDefinition
                        {
                            TaskId = jobVM.Job.Task,
                            Date = JobsDate,
                            Description = jobVM.Description,
                            Duration = (int)jobVM.Job.Duration.TotalMinutes,
                            IsBillable = taskHeader.IsBillable,
                        });

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        jobVM.TaskTitle = "ERROR " + jobVM.TaskTitle;
                    }
                    jobVM.JobId = 1; // @@

                    break;
                }
            }
        }

        #endregion

        public void LoadPlugins()
        {
            CrmPlugins = pluginService.LoadPlugins();
            foreach (var crmVM in CrmPlugins)
            {
                crmVM.PropertyChanged += CrmVM_PropertyChanged;
            }
        }

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
    }
}