using Import.View;
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

namespace Import
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        //This is not MVVM, probably can incorporate import and export to this form, might do that. hahaha
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            ImportView iv = new ImportView();
            iv.Show();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            ExportView ev = new ExportView();
            ev.Show();
        }
    }
}
