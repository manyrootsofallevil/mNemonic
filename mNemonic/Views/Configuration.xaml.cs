using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mNemonic.Forms
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : Window
    {
        System.Windows.Forms.Timer Timer;

        public Configuration()
        {
            Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");
            Timer.Enabled = false;

            InitializeComponent();
            Interval.Text = ConfigurationManager.AppSettings["Interval"];
            CollectStats.IsChecked = Boolean.Parse(ConfigurationManager.AppSettings["CollectStats"]);
            InitialInterval.Text = ConfigurationManager.AppSettings["InitialInterval"];
            CloseOpenPopUp();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");
            Timer.Enabled = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int interval, initialInterval;

            try
            {
                if (Int32.TryParse(Interval.Text.Trim(), out interval) &&
            Int32.TryParse(InitialInterval.Text.Trim(), out initialInterval))
                {
                    // Get the current configuration file.
                    System.Configuration.Configuration config =
                            ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    config.AppSettings.Settings["Interval"].Value = interval.ToString();
                    config.AppSettings.Settings["CollectStats"].Value = CollectStats.IsChecked.ToString();
                    config.AppSettings.Settings["InitialInterval"].Value = initialInterval.ToString();

                    //May as well update the time here.
                    Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");
                    Timer.Interval = interval * 60 * 1000;

                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");

                    this.Close();

                }
                else
                {
                    //Meaningful error message let your user become lazy. don't use them. keep then on their toes
                    MessageBox.Show("An Error occurred. Try Again");
                }
            }
            catch (Exception ex)
            {//TODO: Why is this failing? Permissions? 
                System.Diagnostics.EventLog.WriteEntry(Constants.source, "An error ocurred saving configuration settings. Exception: {0}", System.Diagnostics.EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// This should hopefully be self explanatory
        /// </summary>
        private static void CloseOpenPopUp()
        {
            foreach (var window in App.Current.Windows)
            {
                if (window.GetType().Name.Equals("PopUp", StringComparison.InvariantCultureIgnoreCase))
                {
                    ((PopUp)window).Close();
                }
            }
        }
    }
}
