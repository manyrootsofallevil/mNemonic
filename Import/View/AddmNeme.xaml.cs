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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Import
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ImportView : Window
    {
        AddmNemeViewModel vm;

        public ImportView(string rootDirectory)
        {
            InitializeComponent();
            //The title is no longer used, leaving it here in case i change my mind
            vm = new AddmNemeViewModel(new AddmNemeModel(rootDirectory));

            vm.RequestClose += (s, e) =>
            {
                this.Close();
            };

            vm.SaveAndNew += (s, e) =>
            {
                this.Close();
                ImportView iv = new ImportView(((AddmNemeEventArgs)e).CurrentRootDirectory);
                iv.Show();
            };

            this.DataContext = vm;
        }

    }
}
