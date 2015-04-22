using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace TimesheetParser.ViewModel
{
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public static ViewModelLocator Instance { get; } = new ViewModelLocator();

        public MainViewModel MainVM => SimpleIoc.Default.GetInstance<MainViewModel>();
    }
}
