using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Yusar.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly DependencyResolver _dependencyResolver;

        public App()
        {
            _dependencyResolver = new DependencyResolver();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                //var dialogService = _dependencyResolver.ServiceProvider.GetService(typeof(IDialogService)) as DialogService;
                //var mainVm = _dependencyResolver.ServiceProvider.GetService<MainVm>();
                //var mainWindow = new MainWindow();

                //dialogService?.SetContext(mainVm.GetContext());
                //dialogService?.SetWindow(mainWindow);

                //mainVm.Preparation();
                //mainWindow.DataContext = mainVm;
                //mainWindow.Show();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Standard Migration Client", "Error", ex.Message, TaskDialogIcon.Error);
                Environment.Exit(0);
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }
    }
}
