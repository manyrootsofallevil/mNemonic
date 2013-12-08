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

        public ImportView()
        {
            InitializeComponent();
            //The title is no longer used, leaving it here in case i change my mind
            vm = new AddmNemeViewModel(new AddmNemeModel(string.Empty));
            
            vm.RequestClose += (s, e) =>
            {
                this.Close();
            };

            this.DataContext = vm;
        }
    }
}
