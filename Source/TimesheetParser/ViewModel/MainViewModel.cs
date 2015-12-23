using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Heavysoft.TimesheetParser.PluginInterfaces;
using Microsoft.Practices.ServiceLocation;
using TimesheetParser.Messages;
using TimesheetParser.Services;

namespace TimesheetParser.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private IEnumerable<JobViewModel> jobs;
        private readonly string crmToken = null;
        private bool isConnected;
        private string sourceText;
        private string resultText;
        private bool distributeIdle;
        private DateTime jobsDate;

        public MainViewModel()
        {
            Title = "Timesheet Parser " + Assembly.GetEntryAssembly().GetName().Version;
            JobsDate = DateTime.Now;

            GenerateCommand = new RelayCommand(GenerateCommand_Executed);
            CrmLoginCommand = new RelayCommand(CrmLoginCommand_Executed);
            SubmitJobsCommand = new RelayCommand(SubmitJobs_Executed);

            Messenger.Default.Register<LoginMessage>(this, Connect);
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
        public ICommand CrmLoginCommand { get; set; }
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

        public void LoadPlugins()
        {
            var pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            if (!Directory.Exists(pluginsDir))
            {
                Directory.CreateDirectory(pluginsDir);
            }

            var plugins = new List<CrmPluginViewModel>();

            foreach (var file in Directory.GetFiles(pluginsDir, "*.dll"))
            {
                var assembly = Assembly.LoadFile(file);
                var type = assembly.GetExportedTypes().FirstOrDefault(t => typeof (ICrm).IsAssignableFrom(t) && t.IsClass);

                if (type == null)
                    continue;

                var crmClient = Activator.CreateInstance(type) as ICrm;
                plugins.Add(new CrmPluginViewModel(crmClient));
            }

            CrmPlugins = plugins;

            // @@ taskInfoService = new TaskInfoService(crmClient);
        }

        public async Task CheckConnection()
        {
            if (string.IsNullOrEmpty(crmToken))
                return;

            // @@ IsConnected = await crmClient.Login(crmToken);
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

        private void CrmLoginCommand_Executed()
        {
            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.NavigateTo("CrmLoginPage.xaml");
        }

        private async void Connect(LoginMessage message)
        {
            // @@ passwordHelper.SaveCredential();
            // @@ IsConnected = await crmClient.Login(message.Login, message.Password.ConvertToUnsecureString());
        }

        private async void SubmitJobs_Executed()
        {
            foreach (var jobVM in Jobs)
            {
                // Job is submitted already.
                if (jobVM.Job.JobId != 0)
                    continue;

                foreach (var plugin in CrmPlugins)
                {
                    if (!plugin.Client.IsValidTask(jobVM.Job.Task))
                        continue;

                    // @@ var taskHeader = await taskInfoService.GetTaskHeader(jobVM.Job.Task);
                    // @@ jobVM.TaskTitle = taskHeader.Title;

                    jobVM.IsTaskCopied = true;
                    jobVM.IsDescriptionCopied = true;
                    jobVM.IsDurationCopied = true;
                    // @@ 
                    /*await crmClient.AddJob(new JobDefinition
                    {
                        TaskId = jobVM.Job.Task,
                        Date = JobsDate, Description = jobVM.Description,
                        Duration = (int) jobVM.Job.Duration.TotalMinutes,
                        IsBillable = taskHeader.IsBillable,
                    });*/
                    jobVM.Job.JobId = 1; // @@

                    break;
                }
            }
        }
    }
}