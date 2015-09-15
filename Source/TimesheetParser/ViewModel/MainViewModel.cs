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

namespace TimesheetParser.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private ICrm crmClient;
        private IEnumerable<JobViewModel> jobs;
        private string crmToken = null;
        private bool isConnected;
        private string sourceText;
        private string resultText;
        private bool distributeIdle;

        public MainViewModel()
        {
            Title = "Timesheet Parser " + Assembly.GetEntryAssembly().GetName().Version;

            GenerateCommand = new RelayCommand(GenerateCommand_Executed);
            CrmLoginCommand = new RelayCommand(CrmLoginCommand_Executed);

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

        public string CrmPlugin { get; set; } = "None";
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

        public void LoadPlugins()
        {
            var pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            if (!Directory.Exists(pluginsDir))
            {
                Directory.CreateDirectory(pluginsDir);
            }

            foreach (var file in Directory.GetFiles(pluginsDir, "*.dll"))
            {
                var assembly = Assembly.LoadFile(file);
                var type = assembly.GetExportedTypes().FirstOrDefault(t => typeof (ICrm).IsAssignableFrom(t) && t.IsClass);

                if (type != null)
                {
                    crmClient = Activator.CreateInstance(type) as ICrm;

                    CrmPlugin = Path.GetFileName(file);
                    break;
                }
            }
        }

        public async Task CheckConnection()
        {
            if (string.IsNullOrEmpty(crmToken))
                return;

            IsConnected = await crmClient.Login(crmToken);
        }

        private void GenerateCommand_Executed()
        {
            var parser = new Parser();
            var result = parser.Parse(SourceText, DistributeIdle);
            ResultText = result.Format();

            var mainVM = ViewModelLocator.Current.MainVM;
            mainVM.Jobs = result.Jobs.Where(j => !string.IsNullOrEmpty(j.Task)).Select(j => new JobViewModel(j)).ToList();

            string previousTask = null;
            bool isOdd = false;

            foreach (var jobVM in mainVM.Jobs)
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
            IsConnected = await crmClient.Login(message.Login, message.Password);
        }
    }
}