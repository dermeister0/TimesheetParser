using GalaSoft.MvvmLight.Ioc;
using Microsoft.EntityFrameworkCore;
using TimesheetParser.DataLayer;

#if !DEBUG
using System;
using System.IO;
#endif

namespace TimesheetParser.Business.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => this, true);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CrmLoginViewModel>();
            SimpleIoc.Default.Register<HelpViewModel>();

            CreateDbContext();
        }

        public static ViewModelLocator Current => SimpleIoc.Default.GetInstance<ViewModelLocator>();
        public MainViewModel MainVM => SimpleIoc.Default.GetInstance<MainViewModel>();
        public CrmLoginViewModel CrmLoginVM => SimpleIoc.Default.GetInstance<CrmLoginViewModel>();
        public HelpViewModel HelpVM => SimpleIoc.Default.GetInstance<HelpViewModel>();

        private void CreateDbContext()
        {
            SimpleIoc.Default.Register(() =>
            {
#if DEBUG
                var database = "Development.db";
#else
                var database = Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "TimesheetParser", "Main.db");
#endif

                var builder = new DbContextOptionsBuilder<AppDbContext>();
                builder.UseSqlite($"Data Source={database}");

                return new AppDbContext(builder.Options);
            });
        }
    }
}