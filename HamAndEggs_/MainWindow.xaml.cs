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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace HamAndEggs_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow;
        public string btnCont;
        public MainWindow()
        {
            InitializeComponent();

            AppWindow = this;

            _NavigationFrame.Navigate(new Pages.Tables());
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void GoToMenuPage(object sender, RoutedEventArgs e)
        {
            _NavigationFrame.Navigate(new Pages.Menu());
        }

        public void GoToTablesPage(object sender, RoutedEventArgs e)
        {
            _NavigationFrame.Navigate(new Pages.Tables());
        }

        public string btnContent
        {
            get
            {
                return btnCont;
            }
            set
            {
                btnCont = value;
            }
        }
    }
}
