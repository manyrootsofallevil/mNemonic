using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace mNemonic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon tb;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //initialize NotifyIcon
            tb = (TaskbarIcon)FindResource("MainIcon");
        }

    }
}
