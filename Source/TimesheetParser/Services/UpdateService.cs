using Squirrel;
using System;
using System.Configuration;

namespace TimesheetParser.Services
{
    /// <summary>
    /// Application update service.
    /// </summary>
    public class UpdateService
    {
        private bool updatesChecked;

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

                using (var manager = new UpdateManager(ConfigurationManager.AppSettings["UpdateRoot"]))
                {
                    await manager.UpdateApp();
                }
            }
            catch (Exception)
            {
                // TODO: Write to log.
            }
#endif
        }
    }
}
