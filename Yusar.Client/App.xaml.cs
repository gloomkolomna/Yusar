using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Yusar.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

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
                var mainVm = _dependencyResolver.ServiceProvider.GetService<MainVm>();
                var mainWindow = _dependencyResolver.ServiceProvider.GetService<MainWindow>();

                mainWindow.DataContext = mainVm;
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Wpf", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }
    }
}
