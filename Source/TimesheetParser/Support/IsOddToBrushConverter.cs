using System.Windows;
using System.Windows.Media;

namespace TimesheetParser.Support
{
    public class IsOddToBrushConverter : BooleanConverter<Brush>
    {
        public IsOddToBrushConverter() : base(Application.Current.Resources["LightRowBrush"] as Brush, Application.Current.Resources["DarkRowBrush"] as Brush)
        {           
        }
    }
}
