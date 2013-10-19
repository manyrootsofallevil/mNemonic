using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mNemonic
{
    /// <summary>
    /// Interaction logic for PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        Timer Timer;

        public PopUp(mNeme mNeme)
        {
            Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");
            Timer.Enabled = false;

            InitializeComponent();

            switch (mNeme.Type)
            {

                case mNemeType.Image:

                    ImageToBeDisplayed.Source =
                        new BitmapImage(new Uri(mNeme.Items.Where(x => x.Item2 == FileType.Image).FirstOrDefault().Item1));

                    using (System.IO.StreamReader sw
                        = new System.IO.StreamReader(mNeme.Items.Where(x => x.Item2 == FileType.Text).FirstOrDefault().Item1))
                    {
                        Answer.Text = sw.ReadToEnd(); 
                        Answer.Visibility = System.Windows.Visibility.Hidden;
                    }
                    break;
                case mNemeType.Text:
                    using (System.IO.StreamReader sw
    = new System.IO.StreamReader(mNeme.Items.Where(x => x.Item2 == FileType.Text).FirstOrDefault().Item1))
                    {
                        Answer.Text = sw.ReadToEnd();
                        Answer.Visibility = System.Windows.Visibility.Hidden;
                    }
                    break;

            }
        }

        void Window_Closed(object sender, EventArgs e)
        {
            Timer.Enabled = true;
        }

        private void Dont_Click(object sender, RoutedEventArgs e)
        {
            Answer.Visibility = System.Windows.Visibility.Visible;
        }

        private void Remember_Click(object sender, RoutedEventArgs e)
        {
            //Write to DB
            this.Close();
        }

    }
}
