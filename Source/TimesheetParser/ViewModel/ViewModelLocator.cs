using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using TimesheetParser.Services;

namespace TimesheetParser.ViewModel
{
    internal class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigationService, MyNavigationService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CrmLoginViewModel>();
        }

        public static ViewModelLocator Current => Application.Current.Resources["ViewModelLocator"] as ViewModelLocator;
        public MainViewModel MainVM => SimpleIoc.Default.GetInstance<MainViewModel>();
        public CrmLoginViewModel CrmLoginVM => SimpleIoc.Default.GetInstance<CrmLoginViewModel>();
    }
}