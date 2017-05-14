using System.Windows.Controls;
using System.Windows.Input;

namespace TimesheetParser.View
{
    /// <summary>
    /// Interaction logic for HelpPage.xaml
    /// </summary>
    public partial class HelpPage : Page
    {
        public HelpPage()
        {
            InitializeComponent();
        }

        public ICommand BackButton;
    }
}
