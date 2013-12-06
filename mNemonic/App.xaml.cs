using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
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

            timer = (System.Windows.Forms.Timer)FindResource("Timer");
#if DEBUG
            timer.Interval = 2000;
#else
            //Interval in the config file is in minutes so ...
            timer.Interval = Int32.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000 * 60;
            
            //We wait for a configurable amount of time and then we display the first item, after that
            //it's the regular time. This is to prevent it from say taking 1 hour to show the first item.
            //Probably not the most elegant solution but ......
            Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["InitialInterval"]) * 1000 * 60);

            DisplayTicker(null, null);
#endif
            timer.Tick += DisplayTicker;
            worker = new Worker(ConfigurationManager.AppSettings["Maindirectory"]);

            timer.Start();

        }

        private async void DisplayTicker(object sender, EventArgs e)
        {
            timer.Enabled = false;
            mNeme next = await worker.GetNextItemAsync();
            PopUp popUp = new PopUp(next);
            popUp.SourceInitialized += (s, a) => popUp.WindowState = WindowState.Maximized;
            popUp.Show();
    
        }
    }


}
