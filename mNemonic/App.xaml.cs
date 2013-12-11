using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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
            try
            {
                //initialize NotifyIcon
                tb = (TaskbarIcon)FindResource("MainIcon");

                timer = (System.Windows.Forms.Timer)FindResource("Timer");
#if DEBUG
                timer.Interval = 2000;
#else
            //Interval in the config file is in minutes so ...            
            //We set the timer interval to the initial one here so it displays the first item reasonably quickly
            //the displayticker event then ensures that the Interval is used instead.
            timer.Interval = Int32.Parse(ConfigurationManager.AppSettings["InitialInterval"]) * 1000 * 60;
#endif
                timer.Tick += DisplayTicker;
                worker = new Worker(ConfigurationManager.AppSettings["Maindirectory"]);
                timer.Start();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(Constants.source, string.Format("An error occurred on Startup.\nException: {0}", ex), EventLogEntryType.Error);
                MessageBox.Show("An error occurred.\nmNemonic has to shutdown");
                App.Current.Shutdown();
            }

        }

        private async void DisplayTicker(object sender, EventArgs e)
        {
            try
            {
                timer.Enabled = false;
                //We read from the config file to ensure that it's always up to date as this could be changed 
                //while the app is running.
#if !DEBUG
                if (timer.Interval != Int32.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000 * 60)
                {
                    timer.Interval = Int32.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000 * 60;
                } 
#endif

                mNeme next = await worker.GetNextItemAsync();
                PopUp popUp = new PopUp(next);
                popUp.SourceInitialized += (s, a) => popUp.WindowState = WindowState.Maximized;
                popUp.Show();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(Constants.source, string.Format("An error occurred on getting the next mNeme.\nException: {0}", ex), EventLogEntryType.Error);
            }

        }
    }


}
