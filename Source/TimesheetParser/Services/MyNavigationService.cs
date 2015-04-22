using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Views;

namespace TimesheetParser.Services
{
    public class MyNavigationService : INavigationService
    {
        private string currentPageKey;

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(string pageKey)
        {
            currentPageKey = pageKey;
            GetNavigationService().Navigate(new Uri(pageKey, UriKind.Relative));
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            throw new NotImplementedException();
        }

        public string CurrentPageKey => currentPageKey;

        private NavigationService GetNavigationService()
        {
            var frame = Application.Current.MainWindow.Content as Frame;
            if (frame == null)
                throw new NullReferenceException("Root frame not found.");
            return frame.NavigationService;
        }
    }
}