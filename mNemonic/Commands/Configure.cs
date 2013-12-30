using mNemonic.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mNemonic.Commands
{
    public class Configure : ICommand
    {
        System.Windows.Forms.Timer Timer;
        private State currentState;

        public void Execute(object parameter)
        {
          
            switch (parameter.ToString().ToLowerInvariant())
            {
                case "interval":
                    StopTimer();
                    ConfigurationInterval config = new ConfigurationInterval();
                    config.Show();                    
                    break;
                case "Collections":
                    StopTimer();
                    break;

            }
        }

        private void StopTimer()
        {
            Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");
            currentState = (State)App.Current.FindResource("CurrentState");
            currentState.Paused = true;
            Timer.Enabled = false;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
