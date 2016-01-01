using System.Windows;
using System.Windows.Media;

namespace TimesheetParser.Support
{
    public class IsOddToBrushConverter : BooleanConverter<Brush>
    {
        public IsOddToBrushConverter() : base(SystemColors.ControlLightLightBrush, SystemColors.ControlBrush)
        {           
        }
    }
}
