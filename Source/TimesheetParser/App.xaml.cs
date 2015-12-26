using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
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
        public App()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService, MyNavigationService>();
            SimpleIoc.Default.Register<IDispatchService, DispatchService>();

            SimpleIoc.Default.Register<IPluginService, PluginService>();

            DispatcherHelper.Initialize();
        }
    }
}
