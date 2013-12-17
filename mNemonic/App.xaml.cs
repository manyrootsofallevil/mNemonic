using Hardcodet.Wpf.TaskbarNotification;
using mNemonic.Model;
using mNemonic.ViewModel;
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
        private int initialTimerInterval;
        private int timerInterval;
        private State currentState;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                //initialize NotifyIcon
                tb = (TaskbarIcon)FindResource("MainIcon");
                timer = (System.Windows.Forms.Timer)FindResource("Timer");
                currentState = (State)FindResource("CurrentState");

#if DEBUG
                currentState.IntervalTimer = 2000;
                timer.Interval = currentState.IntervalTimer;
#else
                //Interval in the config file is in minutes so ...            
                //We set the timer interval to the initial one here so it displays the first item reasonably quickly
                //the displayticker event then ensures that the Interval is used instead.
                initialTimerInterval = Int32.Parse(ConfigurationManager.AppSettings["InitialInterval"]) * 1000 * 60;
                currentState.IntervalTimer = Int32.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000 * 60;
                timer.Interval = initialTimerInterval;
#endif
                timer.Tick += DisplayTicker;
                worker = new Worker(ConfigurationManager.AppSettings["Maindirectory"]);
                Helper.UpdateToolTip(tb, timer.Interval);
                //tb.DataContext = new TaskBarIconVM(new TaskBarIconModel( "Initializing" ,timer.Interval));


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
#if !DEBUG
                if (timer.Interval != currentState.IntervalTimer)
                {
                    timer.Interval = currentState.IntervalTimer;
                }
#endif

                mNeme next = await worker.GetNextItemAsync();
                PopUp popUp = new PopUp(next);
                popUp.SourceInitialized += (s, a) => popUp.WindowState = WindowState.Maximized;
                popUp.Show();
            }
            catch (Exception ex)
            {
            }

        }
    }


}
