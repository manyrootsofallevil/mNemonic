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

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //initialize NotifyIcon
            tb = (TaskbarIcon)FindResource("MainIcon");
            //timer = new System.Windows.Forms.Timer();

            timer = (System.Windows.Forms.Timer)FindResource("Timer");

            //Interval is in minutes so ...
#if DEBUG
            timer.Interval = 4000;
#else
            timer.Interval = Int32.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000 * 60;
#endif
            timer.Tick += DisplayTicker;
            timer.Start();
        }

        private async void DisplayTicker(object sender, EventArgs e)
        {
            Worker test = new Worker();
            string bobr = await test.GetNextItemAsync();
            PopUp op = new PopUp(timer);
            op.Show();
        }
    }


}
