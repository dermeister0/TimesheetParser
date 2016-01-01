using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace TimesheetParser.Win10.Controls
{
    public sealed class JobListView : ListView
    {
        public JobListView()
        {
            this.DefaultStyleKey = typeof(JobListView);
        }
    }
}
