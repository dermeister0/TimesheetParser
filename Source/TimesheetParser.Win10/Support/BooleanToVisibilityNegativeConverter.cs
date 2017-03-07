using Windows.UI.Xaml;

namespace TimesheetParser.Win10.Support
{
    public sealed class BooleanToVisibilityNegativeConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityNegativeConverter() :
            base(Visibility.Collapsed, Visibility.Visible)
        {
        }
    }
}