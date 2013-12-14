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

namespace Import.View
{
    /// <summary>
    /// Interaction logic for ConfigurationmNemonic.xaml
    /// </summary>
    public partial class ConfigurationmNemonicView : Window
    {
        string mNemonicexe;

        public ConfigurationmNemonicView()
        {
            InitializeComponent();

            mNemonicexe = System.IO.Path.Combine(ConfigurationManager.AppSettings["mNemonicConfigFile"], "mNemonic.exe"); 

            System.Configuration.Configuration config =
                           ConfigurationManager.OpenExeConfiguration(mNemonicexe);

            Interval.Text = config.AppSettings.Settings["Interval"].Value;
            CollectStats.IsChecked = Boolean.Parse(config.AppSettings.Settings["CollectStats"].Value);
            InitialInterval.Text = config.AppSettings.Settings["InitialInterval"].Value;
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
                            ConfigurationManager.OpenExeConfiguration(mNemonicexe);

                    config.AppSettings.Settings["Interval"].Value = interval.ToString();
                    config.AppSettings.Settings["CollectStats"].Value = CollectStats.IsChecked.ToString();
                    config.AppSettings.Settings["InitialInterval"].Value = initialInterval.ToString();

                    config.Save(ConfigurationSaveMode.Full);
                    ConfigurationManager.RefreshSection("appSettings");

                }
                else
                {
                    //Meaningful error message let your user become lazy. don't use them. keep then on their toes
                    MessageBox.Show("An Error occurred. Try Again");
                }
            }
            catch (Exception ex)
            {

            }

            this.Close();
        }


    }
}
