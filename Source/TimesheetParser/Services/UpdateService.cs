using Squirrel;
using System;
using System.Configuration;

namespace TimesheetParser.Services
{
    /// <summary>
    /// Application update service.
    /// </summary>
    internal sealed class UpdateService : IDisposable
    {
        private bool updatesChecked;
        private UpdateManager updateManager;

        /// <summary>
        /// Downloads new version of application in background.
        /// </summary>
        public async void UpdateApp()
        {
#if !DEBUG
            try
            {
                if (updatesChecked)
                {
                    return;
                }
                updatesChecked = true;

                updateManager = new UpdateManager(ConfigurationManager.AppSettings["UpdateRoot"]);
                await updateManager.UpdateApp();
            }
            catch (Exception)
            {
                // TODO: Write to log.
            }
#endif
        }

        public void Dispose()
        {
            updateManager?.Dispose();
        }
    }
}
