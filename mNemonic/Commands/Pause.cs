using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mNemonic.Commands
{
    public class Pause: ICommand
    {
        System.Windows.Forms.Timer Timer;
        TaskbarIcon tb;
        private State currentState;

        public void Execute(object parameter)
        {
            Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");
            currentState = (State)App.Current.FindResource("CurrentState");
            tb = (TaskbarIcon)App.Current.FindResource("MainIcon"); 

            Timer.Enabled = false;
            currentState.Paused = true;
            Helper.UpdateToolTip(tb, Timer.Interval, true);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    } 
}
