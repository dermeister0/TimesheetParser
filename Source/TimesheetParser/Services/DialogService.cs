using GalaSoft.MvvmLight.Views;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace TimesheetParser.Services
{
    /// <summary>
    /// Service to show messages.
    /// </summary>
    internal class DialogService : IDialogService
    {
        /// <inheritdoc />
        public Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task ShowMessageBox(string message, string title)
        {
            throw new NotImplementedException();
        }
    }
}
