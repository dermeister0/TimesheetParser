using GalaSoft.MvvmLight.Ioc;

namespace TimesheetParser.Business.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel MainVM => SimpleIoc.Default.GetInstance<MainViewModel>();
    }
}