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

namespace HamAndEggs_.Pages
{
    /// <summary>
    /// Interaction logic for Tables.xaml
    /// </summary>
    public partial class Tables : Page
    {
        public string btnCt;
        public Tables()
        {
            InitializeComponent();
        }

        public void Default_Click(object sender, RoutedEventArgs e)
        {
            btnCt = ((Button)sender).Content.ToString();
            MainWindow.AppWindow.btnContent = btnCt;
            MainWindow.AppWindow.GoToMenuPage(sender, e);
        }
    }
}
