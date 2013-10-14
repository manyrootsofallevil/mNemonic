﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mNemonic
{
    /// <summary>
    /// Interaction logic for PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        Timer Timer;

        public PopUp(Timer timer, mNeme mNeme)
        {
            this.Timer = timer;
            Timer.Enabled = false;
            InitializeComponent();
            System.Windows.Forms.MessageBox.Show(mNeme.Location);
        }

        void Window_Closed(object sender, EventArgs e)
        {
            Timer.Enabled = true;
        }
    }
}
