using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mNemonic.Commands
{
    public class Play: ICommand
    {
        System.Windows.Forms.Timer Timer;
        TaskbarIcon tb;

        public void Execute(object parameter)
        {
            Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");
            tb = (TaskbarIcon)App.Current.FindResource("MainIcon");
            Helper.UpdateToolTip(tb, Timer.Interval);
            Timer.Enabled = true;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    } 
}
