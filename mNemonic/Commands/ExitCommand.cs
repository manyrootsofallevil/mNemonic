using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mNemonic.Commands
{
    /// <summary>
    /// A simple command that displays the command parameter as
    /// a dialog message.
    /// </summary>
    public class ExitCommand : ICommand
    {
        public void Execute(object parameter)
        {
            PopUp pop = new PopUp();
            pop.Show();

           // App.Current.Shutdown();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

      public event EventHandler CanExecuteChanged;
    } 
}
