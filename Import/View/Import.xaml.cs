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
        ImportViewModel vm;

        public ImportView()
        {
            InitializeComponent();

            if (App.args.Length > 0)
            {
                switch (App.args[0].ToLower())
                {
                    case "-i": vm = new ImportViewModel(new ImportModel("Import")); break;
                    case "-e": vm = new ImportViewModel(new ImportModel("Export")); break;
                }
            }

            vm.RequestClose += (s, e) =>
            {
                this.Close();
            };

            this.DataContext = vm;
        }
    }
}
