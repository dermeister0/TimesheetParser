using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace TimesheetParser.Win10.Support
{
    public class IsOddToBrushConverter : BooleanConverter<Brush>
    {
        public IsOddToBrushConverter() : base(Application.Current.Resources["SystemControlBackgroundChromeMediumLowBrush"] as Brush, Application.Current.Resources["SystemControlBackgroundChromeMediumBrush"] as Brush)
        {
        }
    }
}
