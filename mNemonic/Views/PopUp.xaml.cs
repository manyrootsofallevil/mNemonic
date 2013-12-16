using Hardcodet.Wpf.TaskbarNotification;
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
        PopUpVM vm;
        private TaskbarIcon tb;

        public PopUp(mNeme mNeme)
        {
            Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");
            Timer.Enabled = false;

            InitializeComponent();

            vm = new PopUpVM(new PopUpModel(mNeme));
            vm.RequestClose += (s, e) =>
            {
                Timer.Enabled = true;
                this.Close();
            };

            this.DataContext = vm;
        }
         //This seems like the simpler solution
        private void Window_Closed(object sender, EventArgs e)
        {
            Timer.Enabled = true;
            //This is not exactly the way I envisioned it in my mind but it should do the trick;
            tb = (TaskbarIcon)FindResource("MainIcon");
            Helper.UpdateToolTip(tb, Timer.Interval);
        }

    }
}
