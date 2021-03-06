﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace mNemonic.Commands
{
    public class Exit : ICommand
    {
        public void Execute(object parameter)
        {
            App.Current.Shutdown();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

      public event EventHandler CanExecuteChanged;
    } 
}
