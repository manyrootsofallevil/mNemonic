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

namespace mNemonic
{
    /// <summary>
    /// Interaction logic for PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        public PopUp()
        {
            InitializeComponent();

            //ImageSource source = new BitmapImage(new Uri(@"C:\Users\John\Desktop\images.jpg"));
            ImageSource source = new BitmapImage(new Uri(@"C:\Users\John\Desktop\07.PNG"));
            Image.Source = source;
        }
    }
}
