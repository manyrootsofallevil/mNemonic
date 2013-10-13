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
        public void Execute(object parameter)
        {
            PopUp pop = new PopUp(null);
            pop.Show();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
