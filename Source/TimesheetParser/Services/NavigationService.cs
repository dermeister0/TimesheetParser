using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TimesheetParser.Business.Services;

namespace TimesheetParser.Services
{
    internal class NavigationService : IPortableNavigationService
    {
        private readonly Dictionary<Location, Uri> pagesByKey;

        public NavigationService()
        {
            pagesByKey = new Dictionary<Location, Uri>
                {
                    [Location.Main] = new Uri("/View/MainPage.xaml", UriKind.Relative),
                    [Location.Login] = new Uri("/View/CrmLoginPage.xaml", UriKind.Relative)
                };
        }

        public void NavigateTo(Location pageKey)
        {
            GetNavigationService().Navigate(pagesByKey[pageKey]);
        }

        private System.Windows.Navigation.NavigationService GetNavigationService()
        {
            var frame = Application.Current.MainWindow.Content as Frame;
            if (frame == null)
                throw new NullReferenceException("Root frame not found.");
            return frame.NavigationService;
        }
    }
}