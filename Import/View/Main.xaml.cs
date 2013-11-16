using Import.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.IO.Compression;
using ioPath = System.IO.Path;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.DefaultExt = ".zip";
            opf.Filter = "Zip Files|*.zip";

            bool? result = opf.ShowDialog();

            if (result == true)
            {
                string root = ConfigurationManager.AppSettings["RootDirectory"];

                try
                {
                    ZipFile.ExtractToDirectory(opf.FileName, ioPath.Combine(root, ioPath.GetFileNameWithoutExtension(opf.FileName)));
                }
                catch (Exception ex)
                {
                    //TODO:
                }
            }

        }
    }
}
