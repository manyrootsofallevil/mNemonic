﻿using System;
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
            int result;

            if (Int32.TryParse(Interval.Text.Trim(), out result))
            {
                // Get the current configuration file.
                System.Configuration.Configuration config =
                        ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);


                config.AppSettings.Settings["Interval"].Value = result.ToString();
                config.AppSettings.Settings["CollectStats"].Value = CollectStats.IsChecked.ToString();
                //May as well update the time here.
                Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");
                Timer.Interval = result * 60 * 1000;

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
