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

        Border currentGrid;
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        SQLiteDataReader sqlite_datareader;
        private ObservableCollection<string> dinList = new ObservableCollection<string>();
        private ObservableCollection<string> pcbList = new ObservableCollection<string>();
        private ObservableCollection<string> readKeyList = new ObservableCollection<string>();
        string availabilty = null;
        string checkProductValue;
        bool checkCombo = false;

        public MainWindow()
        {
            InitializeComponent();
            addDinValues();
            addPCBValues();
            addRKValues();

            // create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=supportInventory.db;Version=3;New=False;Compress=True;");

            // create a new SQL command:
            sqlite_cmd = sqlite_conn.CreateCommand(); 

        }

        private void addDinValues()
        {
            dinList.Add("CRX-POSTX-DIN");
            dinList.Add("CRX-POSTX-DIN-GP");
            dinList.Add("CRX-POSTX-DIN-WF");
            dinList.Add("CRX-POSTX-DIN-WFGP");
            dinList.Add("PRT-ADC4-DIN");
            dinList.Add("PRT-CTRL-DIN");
            dinList.Add("PRT-DAC4-DIN");
            dinList.Add("PRT-IO84-DIN");
            dinList.Add("PRT-PSU-DIN-2A");
            dinList.Add("PRT-PSU-DIN-2x");
            dinList.Add("PRT-PSU-DIN-4A");
            dinList.Add("PRT-PX8-DIN");
            dinList.Add("PRT-RDM2-DIN");
            dinList.Add("PRT-WX-DIN");
            dinList.Add("PRT-ZX16-DIN");
        }

        private void addPCBValues()
        {
            pcbList.Add("ACC-485-USB");
            pcbList.Add("ACC-PSU-5A");
            pcbList.Add("CRF-FOX-GSM");
            pcbList.Add("CRX-POSTX");
            pcbList.Add("PRT-ADC4-PCB");
            pcbList.Add("PRT-COMM");
            pcbList.Add("PRT-CTRL-GX");
            pcbList.Add("PRT-CTRL-LE");
            pcbList.Add("PRT-CTRL-SE");
            pcbList.Add("PRT-DAC4-PCB");
            pcbList.Add("PRT-ECD");
            pcbList.Add("PRT-KIT-LE-ALM-AU");
            pcbList.Add("PRT-KIT-LE-ALM-CA");
            pcbList.Add("PRT-KIT-LE-ALM-NZ");
            pcbList.Add("PRT-PSU-5I");
            pcbList.Add("PRT-PX16-DRI");
            pcbList.Add("PRT-PX16-PCB");
            pcbList.Add("PRT-PXS16-PCB");
            pcbList.Add("PRT-RDE2-PCB");
            pcbList.Add("PRT-RDI2-PCB");
            pcbList.Add("PRT-RDM2-PCB");
            pcbList.Add("PRT-DAC4-PCB");
            pcbList.Add("PRT-RDS2-PCB");
            pcbList.Add("PRT-SE-RACK");
            pcbList.Add("PRT-ZX16-PCB");
            pcbList.Add("PRT-ZXS16-PCB");
            pcbList.Add("PRX-SAM");
            pcbList.Add("RF-RCVR-433");
            pcbList.Add("RLY-DP");
            pcbList.Add("RLY-SP");
            ProductComboBox.ItemsSource = pcbList;
        }

        private void addRKValues()
        {
            readKeyList.Add("CRX-POSTX-DIN");
            readKeyList.Add("CRX-POSTX-DIN-GP");
            readKeyList.Add("CRX-POSTX-DIN-WF");
            readKeyList.Add("CRX-POSTX-DIN-WFGP");
            readKeyList.Add("PRT-ADC4-DIN");
            readKeyList.Add("PRT-CTRL-DIN");
            readKeyList.Add("PRT-DAC4-DIN");
            readKeyList.Add("PRT-IO84-DIN");
            readKeyList.Add("PRT-PSU-DIN-2A");
            readKeyList.Add("PRT-PSU-DIN-2x");
            readKeyList.Add("PRT-PSU-DIN-4A");
            readKeyList.Add("PRT-PX8-DIN");
            readKeyList.Add("PRT-RDM2-DIN");
            readKeyList.Add("PRT-WX-DIN");
            readKeyList.Add("PRT-ZX16-DIN");
            ProductComboBox.ItemsSource = readKeyList;
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

            // check what type box was used
            if (checkCombo)
            {
                checkProductValue = ProductComboBox.Text;
            }
            else
            {
                checkProductValue = ProductTextBox.Text;
            }

            // check all fields are not null if null notify user.
            if (check_Fields())
            {

                sqlite_conn.Open();

                sqlite_cmd.ExecuteNonQuery();

                sqlite_cmd.CommandText = "INSERT INTO Items (ProductType, SerialNumber, Location, Availability, ETR) VALUES ('" + checkProductValue + "','" + this.SerialBox.Text + "','" + this.LocationBox.Text + "','" + availabilty + "','" + this.ETRBox.Text + "');";

                sqlite_cmd.ExecuteNonQuery();
                
                sqlite_conn.Close();
                

            }
            else
            {
            }

        }

        private void productTypeClick(object sender, RoutedEventArgs e)
        {

            string nameOfButton = (sender as RadioButton).Name;
            if (nameOfButton == "Din")
            {
                ProductTextBox.Visibility = Visibility.Collapsed;
                ProductComboBox.ItemsSource = dinList;
                ProductComboBox.Visibility = Visibility.Visible;
                typeLabel.Content = "DIN";
                checkCombo = true;
            }
            else if (nameOfButton == "PCB")
            {
                ProductTextBox.Visibility = Visibility.Collapsed;
                ProductComboBox.ItemsSource = pcbList;
                ProductComboBox.Visibility = Visibility.Visible;
                typeLabel.Content = "PCB";
                checkCombo = true;
            }
            else if (nameOfButton == "ReaderKeypad")
            {
                ProductTextBox.Visibility = Visibility.Collapsed;
                ProductComboBox.ItemsSource = readKeyList;
                ProductComboBox.Visibility = Visibility.Visible;
                typeLabel.Content = "Reader/Keypad";
                checkCombo = true;
            }
            else if (nameOfButton == "Other")
            {
                ProductComboBox.Visibility = Visibility.Collapsed;
                ProductTextBox.Visibility = Visibility.Visible;
                typeLabel.Content = "Other";
                checkCombo = false;
            }


        }

        private bool check_Fields()
        {
            bool nullValues = true;
            if (checkProductValue == null)
            {
                nullValues = false;
            }
            else if (SerialBox.Text == null)
            {
                nullValues = false;
            }
            else if (LocationBox.Text == null)
            {
                nullValues = false;
            }
            else if (availabilty == null)
            {
                nullValues = false;
            }
            else if (ETRBox.Text == null)
            {
                nullValues = false;
            }
            return nullValues;
        }

        private void check_Click(object sender, RoutedEventArgs e)
        {
            string val = (sender as RadioButton).Name;
            if (val == "checkYes")
            {
                availabilty = "Yes";
                DuebackPanel.Visibility = Visibility.Collapsed; 
                dueBackLabel.Visibility = Visibility.Collapsed;
            }
            else if (val == "checkNo")
            {
                availabilty = "No";
                dueBackLabel.Visibility = Visibility.Visible;
                DuebackPanel.Visibility = Visibility.Visible;
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



