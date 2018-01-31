using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Collections;
using System.IO;

namespace HamAndEggs_.Pages
{

    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        string strConnection = ConfigurationManager.ConnectionStrings["KFCConnection"].ConnectionString;
        string CmdString = string.Empty;
        public string Tafel;

        Ham_and_EggsDataSet datasource = new Ham_and_EggsDataSet();
        public string Data;

        public string text;
        public Menu()
        {
            InitializeComponent();
            FillDataGrid("SELECT * FROM Food WHERE Type='Voorgerecht'" , "Food" , myDataGrid);
            Tafel = MainWindow.AppWindow.btnCont;
            FillDataGrid($"SELECT * FROM Receipt WHERE Tafel='{Tafel}'", "Receipt", ReceiptGrid);
        }

        private void FillDataGrid(string Cmd, string Table, DataGrid grid)
        {
            using (SqlConnection sqlConnection = new SqlConnection(strConnection))
            {
                CmdString = Cmd;
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnection);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = datasource.Tables[Table];
                sda.Fill(dt);
                grid.ItemsSource = dt.DefaultView;
            }


        }

        private void InsertIntoDatabase(string Order, string Price , string Tafel)
        {

            using (SqlConnection sqlConnection = new SqlConnection(strConnection))
            {
                CmdString = ($"INSERT INTO Receipt (Name, Price, Tafel) VALUES ('{Order}' , '{Price}' , '{Tafel}')");
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnection);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = datasource.Tables["Receipt"];
                sda.Fill(dt);
            }
        }

        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string Order;
            string Price;
            DataRowView drv = (DataRowView)myDataGrid.SelectedItem;
            if (drv != null)
            { 
                Order = (drv[1].ToString());
                Price = (drv[2].ToString());
            }
            else
                return;
            string Tafel = MainWindow.AppWindow.btnCont;
            InsertIntoDatabase(Order , Price , Tafel);
            FillDataGrid($"SELECT * FROM Receipt WHERE Tafel='{Tafel}'" , "Receipt", ReceiptGrid);
        }
        public class Receip
        {
            public int Amount { get; set; }
            public string Order { get; set; }
            public string Price { get; set; }

        }
        private void btnReceipt_Click(object sender, RoutedEventArgs e)
        {
            List<Receip> receipt = new List<Receip>();
            //var receipt = new List<string>();
            using (SqlConnection sqlConnection = new SqlConnection(strConnection))
            {
                CmdString = ($"SELECT * FROM Receipt WHERE Tafel='{Tafel}'");
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnection);
                DataTable dt = datasource.Tables["Receipt"];
                SqlDataReader dr;
                sqlConnection.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string ordered = dr.GetString(dr.GetOrdinal("Name"));
                    int check = -1;

                    if (receipt.Count != 0)
                    {
                        for (int x = 0; x < receipt.Count; x++)
                        {
                            if (receipt[x].Order == ordered)
                            {
                                check = x;
                            }
                        }
                    }
                    if (check > -1)
                    {
                        int OrderPrice;
                        receipt[check].Amount = receipt[check].Amount + 1;
                        OrderPrice = Convert.ToInt16(dr.GetString(dr.GetOrdinal("Price")));
                        int total = OrderPrice * receipt[check].Amount;
                        receipt[check].Price = Convert.ToString(total);
                    }
                    else
                    {
                        receipt.Add(new Receip()
                        {
                            Amount = 1,
                            Order = dr.GetString(dr.GetOrdinal("Name")),
                            Price = dr.GetString(dr.GetOrdinal("Price")),
                        });
                    }
                }


                dr.Close();
                sqlConnection.Close();
            }

            //string[] text = receipt.ToArray();
            //string[] text;
            if (!File.Exists($"../Receipts/{Tafel} Receipt.txt"))
            {
                File.WriteAllText($"../Receipts/{Tafel} Receipt.txt", "");
            }
            string[] strText = new string[] { DateTime.Now.ToLongDateString() , DateTime.Now.ToLongTimeString()};
            int i = 0;
            for (int x = 0; x < receipt.Count; x++)
            {                
                string[] Text = new string[] { Convert.ToString(receipt[x].Amount) + "x", receipt[x].Order , "€" + receipt[x].Price + ".00"};
                text = string.Join(" ",Text);
                Array.Resize(ref strText, strText.Length + 1);
                for (i = 0; i < strText.Length; i++) { }
                strText[i - 1] = text;
                File.WriteAllLines($"../Receipts/{Tafel} Receipt.txt", strText);
            }
            
            btnClearReceipt_Click(sender,e);
        }

        private void btnReserveTable_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClearReceipt_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection(strConnection))
            {
                CmdString = ($"DELETE FROM Receipt WHERE Tafel='{Tafel}'");
                SqlCommand cmd = new SqlCommand(CmdString, sqlConnection);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = datasource.Tables["Receipt"];
                sda.Fill(dt);
            }
            MainWindow.AppWindow.GoToTablesPage(sender, e);
        }

        private void btnVoorgerecht_Click(object sender, RoutedEventArgs e)
        {
            string btnCt = ((Button)sender).Content.ToString();
            datasource.Clear();
            FillDataGrid($"SELECT * FROM Food Where Type='{btnCt}'" , "Food" , myDataGrid);
            FillDataGrid($"SELECT * FROM Receipt WHERE Tafel='{Tafel}'", "Receipt", ReceiptGrid);
        }
    }
}
