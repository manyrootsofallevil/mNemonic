using mNemonic.Model;
using mNemonic.ViewModel;
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
        PopUpVM vm = new PopUpVM(new PopUpModel());
        //

        public PopUp(mNeme mNeme)
        {
            Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");
            Timer.Enabled = false;

            InitializeComponent();
            
            vm.RequestClose += (s, e) => this.Close();
            
            this.DataContext = vm;

            switch (mNeme.Type)
            {

                case mNemeType.Image:

                    ImageToBeDisplayed.Source =
                        new BitmapImage(new Uri(mNeme.Items.Where(x => x.Item2 == FileType.Image).FirstOrDefault().Item1));

                    using (System.IO.StreamReader sw
                        = new System.IO.StreamReader(mNeme.Items.Where(x => x.Item2 == FileType.Text).FirstOrDefault().Item1))
                    {
                       // Answer.Text = sw.ReadToEnd();
                        //Answer.Visibility = System.Windows.Visibility.Hidden;
                    }
                    break;
                case mNemeType.Text:
                    using (System.IO.StreamReader sw
    = new System.IO.StreamReader(mNeme.Items.Where(x => x.Item2 == FileType.Text
        && !x.Item1.ToLower().Contains("question")).FirstOrDefault().Item1))
                    {
                        //Answer.Text = sw.ReadToEnd();
                        //Answer.Visibility = System.Windows.Visibility.Hidden;
                    }
                    using (System.IO.StreamReader sw
    = new System.IO.StreamReader(mNeme.Items.Where(x => x.Item2 == FileType.Text
        && x.Item1.ToLower().Contains("question")).FirstOrDefault().Item1))
                    {
                        //Question.Text = sw.ReadToEnd();
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
