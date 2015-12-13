using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimesheetParser.Win10.Support
{
    public class RtfText
    {
        public static string GetRichText(DependencyObject obj)
        {
            return (string)obj.GetValue(RichTextProperty);
        }

        public static void SetRichText(DependencyObject obj, string value)
        {
            obj.SetValue(RichTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for RichText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RichTextProperty =
            DependencyProperty.RegisterAttached("RichText", typeof(string), typeof(RtfText), new PropertyMetadata(null, Callback));

        private static void Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var reb = (RichEditBox)d;

            if (e.NewValue != null && e.OldValue == null)
            {
                reb.Document.SetText(TextSetOptions.FormatRtf, (string)e.NewValue);

                reb.TextChanged += (sender, args) => 
                {
                    string text;
                    reb.Document.GetText(TextGetOptions.AdjustCrlf, out text);
                    d.SetValue(RtfText.RichTextProperty, text);
                };
            }
        }
    }
}
