using System;
using GalaSoft.MvvmLight.Threading;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Services
{
    internal class DispatchService : IDispatchService
    {
        public void InvokeOnUIThread(Action action)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(action);
        }
    }
}
