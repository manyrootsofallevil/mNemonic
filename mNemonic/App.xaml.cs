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
        private System.Windows.Forms.Timer timer;
        private Worker worker;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //initialize NotifyIcon
            tb = (TaskbarIcon)FindResource("MainIcon");
            //timer = new System.Windows.Forms.Timer();

            timer = (System.Windows.Forms.Timer)FindResource("Timer");
#if DEBUG
            timer.Interval = 3000;
#else
            //Interval in the config file is in minutes so ...
            timer.Interval = Int32.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000 * 60;
#endif
            timer.Tick += DisplayTicker;
            worker = new Worker(ConfigurationManager.AppSettings["Maindirectory"]);
            timer.Start();
        }

        private async void DisplayTicker(object sender, EventArgs e)
        {
            mNeme next = await worker.GetNextItemAsync();
            PopUp popUp = new PopUp(next);
            popUp.Show();
        }
    }


}
