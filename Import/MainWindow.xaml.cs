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
    public partial class MainWindow : Window
    {
        ViewModel vm;

        public MainWindow()
        {
            InitializeComponent();

            if (App.args.Length > 0)
            {
                switch (App.args[0].ToLower())
                {
                    case "-i": vm = new ViewModel(new Model("Import")); break;
                    case "-e": vm = new ViewModel(new Model("Export")); break;
                }
            }
            this.DataContext = vm;
        }
    }
}
