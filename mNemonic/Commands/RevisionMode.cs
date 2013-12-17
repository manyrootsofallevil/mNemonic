using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mNemonic.Commands
{
    public class RevisionMode : ICommand
    {
        System.Windows.Forms.Timer Timer;
        private State currentState;

        public void Execute(object parameter)
        {
            Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");
            currentState = (State)App.Current.FindResource("CurrentState");

            if (Boolean.Parse(parameter.ToString()))
            {
                currentState.IntervalTimer = 250;
                Timer.Interval = currentState.IntervalTimer;
            }
            else
            {
                currentState.IntervalTimer = Int32.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000 * 60;
                Timer.Interval = currentState.IntervalTimer;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
