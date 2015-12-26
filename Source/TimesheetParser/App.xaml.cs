using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using TimesheetParser.Business.Services;
using TimesheetParser.Services;

namespace TimesheetParser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService, MyNavigationService>();
            SimpleIoc.Default.Register<IPluginService>(() => new PluginService());
        }
    }
}
