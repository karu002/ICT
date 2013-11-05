using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Finisar.SQLite;

namespace SupportInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;
        Border currentGrid;
        private ObservableCollection<string> comboList = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();

            // create combo-box values
            addValues();

            // create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=supportInventory.db;Version=3;New=False;Compress=True;");
            // open the connection:
            sqlite_conn.Open();
            // create a new SQL command:
            sqlite_cmd = sqlite_conn.CreateCommand();

            // create new table once initialised
            sqlite_cmd.CommandText = "CREATE TABLE Items (ProductType varchar(100), SerialNumber varchar(100), Location varchar(100), Availability varchar(150), ETR varchar(100));";

            // We are ready, now lets cleanup and close our connection:
            sqlite_conn.Close();
        }

        private void addValues()
        {
            comboList.Add("CRX-POSTX-DIN");
            comboList.Add("CRX-POSTX-DIN-GP");
            comboList.Add("CRX-POSTX-DIN-WF");
            comboList.Add("CRX-POSTX-DIN-WFGP");
            comboList.Add("PRT-ADC4-DIN");
            comboList.Add("PRT-CTRL-DIN");
            comboList.Add("PRT-DAC4-DIN");
            comboList.Add("PRT-IO84-DIN");
            comboList.Add("PRT-PSU-DIN-2A");
            comboList.Add("PRT-PSU-DIN-2x");
            comboList.Add("PRT-PSU-DIN-4A");
            comboList.Add("PRT-PX8-DIN");
            comboList.Add("PRT-RDM2-DIN");
            comboList.Add("PRT-WX-DIN");
            comboList.Add("PRT-ZX16-DIN");
            ProductTypeBox.ItemsSource = comboList;
        }

        private void AllItems_Click(object sender, RoutedEventArgs e)
        {
            currentGrid = AllItem;
            currentGrid.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Collapsed;
            HeaderLabel.Content = "All Items";
            BackButton.Visibility = Visibility.Visible;
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            currentGrid = AddItem;
            currentGrid.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Collapsed;
            HeaderLabel.Content = "Add a New Item";
            BackButton.Visibility = Visibility.Visible;
        }

        private void Available_Click(object sender, RoutedEventArgs e)
        {
            currentGrid = AvailableItem;
            currentGrid.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Collapsed;
            HeaderLabel.Content = "Available Items";
            BackButton.Visibility = Visibility.Visible;
        }

        private void Unavailable_Click(object sender, RoutedEventArgs e)
        {
            currentGrid = UnavailableItem;
            currentGrid.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Collapsed;
            HeaderLabel.Content = "Unavailable Items";
            BackButton.Visibility = Visibility.Visible;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Visible;
            currentGrid.Visibility = Visibility.Collapsed;
            HeaderLabel.Content = "Support Inventory";
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void SubmitType_Click(object sender, RoutedEventArgs e)
        {
            if (ProductTypeBox.Text == null || SerialBox.Text == null || LocationBox.Text == null || ETRBox == null)
            {
                //open connection for writing
                sqlite_conn.Open();

                // Now lets execute the SQL ;D
                sqlite_cmd.ExecuteNonQuery();

                // Lets insert something into our new table:
                sqlite_cmd.CommandText = "INSERT INTO Items (ProductType, SerialNumber, Location, ETR) VALUES ('" + this.ProductTypeBox.Text + "','" + this.SerialBox.Text + "','" + this.LocationBox.Text + "','" + ETRBox.Text + "');";

                // And execute this again ;D
                sqlite_cmd.ExecuteNonQuery();

                // We are ready, now lets cleanup and close our connection:
                sqlite_conn.Close();
            }
            else
            {
                MessageBox.Show("Please fill in all the fields");
            }


        }

    }
}



            //// But how do we read something out of our table ?
            //// First lets build a SQL-Query again:
            //sqlite_cmd.CommandText = "SELECT * FROM Items";

            //// Now the SQLiteCommand object can give us a DataReader-Object:
            //sqlite_datareader = sqlite_cmd.ExecuteReader();

            //// The SQLiteDataReader allows us to run through the result lines:
            //while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            //{
            //    // Print out the content of the text field:
            //    //System.Console.WriteLine(sqlite_datareader["text"]);
            //    string myVar = sqlite_datareader.GetString(1);
            //    MessageBox.Show(myVar);
            //}