using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TimesheetParser.Business.ViewModel;

namespace TimesheetParser.Support
{
    /// <summary>
    /// Converts job status to job title brush.
    /// </summary>
    [ValueConversion(typeof(JobStatus), typeof(Brush))]
    internal class JobStatusToBrushConverter : IValueConverter
    {
        private readonly Brush blackBrush;
        private readonly Brush greenBrush;
        private readonly Brush redBrush;

        /// <summary>
        /// Constructor.
        /// </summary>
        public JobStatusToBrushConverter()
        {
            blackBrush = Application.Current.Resources["BlackBrush"] as Brush;
            greenBrush = Application.Current.Resources["GreenBrush"] as Brush;
            redBrush = Application.Current.Resources["RedBrush"] as Brush;
        }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is JobStatus status)
            {
                switch (status)
                {
                    case JobStatus.Success:
                        return greenBrush;

                    case JobStatus.Failure:
                        return redBrush;

                    default:
                        return blackBrush;
                }
            }

            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
