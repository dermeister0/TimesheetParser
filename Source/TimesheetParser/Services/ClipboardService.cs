using System;
using System.Windows;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Services
{
    class ClipboardService : IClipboardService
    {
        public void SetText(string text)
        {
            try
            {
                Clipboard.SetText(text, TextDataFormat.UnicodeText);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to set clipboard text.");
            }
        }
    }
}
