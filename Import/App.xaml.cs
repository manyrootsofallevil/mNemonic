﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Import
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] args;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string title = string.Empty;
            
            if (e.Args.Length >0)
            {
                args = e.Args;
            }
        }
    }
}
