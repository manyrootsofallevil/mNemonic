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

        public void Execute(object parameter)
        {
            Timer = (System.Windows.Forms.Timer)App.Current.FindResource("Timer");

            if (Boolean.Parse(parameter.ToString()))
            {
                Timer.Interval = 250;
            }
            else
            {
                Timer.Interval = Int32.Parse(ConfigurationManager.AppSettings["Interval"]) * 1000 * 60;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
