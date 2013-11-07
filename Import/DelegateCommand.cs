using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace mNemonic.Commands
{
    public class DelegateCommand : ICommand
    {
        Func<object, bool> canExecute;
        bool canExecuteCache;

        Action<object> execute;

        public DelegateCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            bool tmp = canExecute(parameter);

            if (canExecuteCache != tmp)
            {
                canExecuteCache = tmp;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }

            return canExecuteCache;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                if (Application.Current.Dispatcher != null && !Application.Current.Dispatcher.CheckAccess())
                {
                    Application.Current.Dispatcher.BeginInvoke
                    (
                        new Action(() => CanExecuteChanged(this, new EventArgs()))
                    );
                }
                else
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
