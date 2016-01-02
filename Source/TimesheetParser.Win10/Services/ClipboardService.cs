using Windows.ApplicationModel.DataTransfer;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Win10.Services
{
    internal class ClipboardService : IClipboardService
    {
        public void SetText(string text)
        {
            var package = new DataPackage();
            package.SetText(text);
            Clipboard.SetContent(package);
        }
    }
}
