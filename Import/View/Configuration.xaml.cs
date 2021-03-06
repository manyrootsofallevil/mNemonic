﻿using Import.Model;
using Import.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Import.View
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class ConfigurationView : Window
    {
        ConfigurationViewModel vm;

        public ConfigurationView()
        {
            InitializeComponent();

            vm = new ConfigurationViewModel(new ConfigurationModel());

            vm.RequestClose += (s, e) =>
            {
                this.Close();
            };

            this.DataContext = vm;
        }
    }
}
