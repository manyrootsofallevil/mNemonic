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

        public void Execute(object parameter)
        {
            //System.Windows.Forms.Timer Timer = (System.Windows.Forms.Timer)parameter;
            Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");

            Timer.Enabled = false;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    } 
}
