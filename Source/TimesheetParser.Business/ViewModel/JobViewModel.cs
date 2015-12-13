using GalaSoft.MvvmLight;
using TimesheetParser.Model;

namespace TimesheetParser.Business.ViewModel
{
    public class JobViewModel : ViewModelBase
    {
        private readonly Job job;
        private string taskTitle;

        public JobViewModel(Job job)
        {
            this.job = job;
        }

        public string Task => "#" + job.Task;
        public string Duration => job.TimeDescription;
        public string Description => job.Description;

        public bool IsOdd { get; set; }

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