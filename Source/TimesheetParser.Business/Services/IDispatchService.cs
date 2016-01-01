using System;

namespace TimesheetParser.Business.Services
{
    public interface IDispatchService
    {
        void InvokeOnUIThread(Action action);
    }
}
