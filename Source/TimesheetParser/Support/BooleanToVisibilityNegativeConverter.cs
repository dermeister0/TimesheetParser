using System.Windows;

namespace TimesheetParser.Support
{
    /// <summary>
    /// Converts true to collapsed and false to visible.
    /// </summary>
    public sealed class BooleanToVisibilityNegativeConverter : BooleanConverter<Visibility>
    {
        public BooleanToVisibilityNegativeConverter() :
            base(Visibility.Collapsed, Visibility.Visible)
        {
        }
    }
}
