using KobukiCore.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KobukiWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Project Properties
        public const string CameraCaptureLink = "http://127.0.0.1:8889/stream.mjpg";

        public static App ApplicationInstance { get; private set; }

        public static KobukiService RobotService { get; private set; }
        #endregion
        public App()
        {
            ApplicationInstance = this;
            RobotService = new KobukiService(enableSkeleton:false);
            RobotService.StartWorkers();
        }

        private void Application_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            RobotService.CloseWorkers();
        }
    }
}
