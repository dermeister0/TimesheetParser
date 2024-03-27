﻿using System;
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

            SimpleIoc.Default.Register<IPortableNavigationService, NavigationService>();
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IPluginService, PluginService>();
            SimpleIoc.Default.Register<IClipboardService, ClipboardService>();
            SimpleIoc.Default.Register<UpdateService, UpdateService>();

            DispatcherHelper.Initialize();

            DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ShowError(e.Exception.ToString());
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowError(e.ExceptionObject.ToString());
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            SimpleIoc.Default.GetInstance<UpdateService>().Dispose();
        }

        private void ShowError(string message)
        {
            MessageBox.Show($"{message}\n\nhttps://github.com/dermeister0/TimesheetParser/issues", "Error");
        }
    }
}
