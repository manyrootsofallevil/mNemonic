﻿using Import.View;
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
using System.Xml.Linq;

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
        //Came to think about it, it might actually be a good idea to add them import, export and add form (which needs renaming) to 
        //this form, each in a separate tab or something like that.
        //However, this time i will get everything working as i think it should and then start looking at prettifying it.
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            ImportView iv = new ImportView(string.Empty);
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
                string collections = ConfigurationManager.AppSettings["CollectionsFile"];

                try
                {
                    ZipFile.ExtractToDirectory(opf.FileName, ioPath.Combine(root, ioPath.GetFileNameWithoutExtension(opf.FileName)));

                    XDocument xdoc = XDocument.Load(collections);

                    xdoc.Root.Add(new XElement("collection", new XAttribute("Name", ioPath.GetFileNameWithoutExtension(opf.FileName)),
                   new XAttribute("relativePath", ioPath.GetFileNameWithoutExtension(opf.FileName)), new XAttribute("Enabled", false)));

                    xdoc.Save(collections);

                }
                catch (Exception ex)
                {
                
                }
            }

        }

        private void Configure_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationView cv = new ConfigurationView();
            cv.Show();
        }

        private void ConfiguremNemonic_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationmNemonicView cv = new ConfigurationmNemonicView();
            cv.Show();
        }
    }
}
