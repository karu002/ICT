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
using System.Data;
using Finisar.SQLite;
using System.Windows.Controls.Primitives;

/// Author: Kirushi Arunthavasothy

namespace SupportInventory
{
    /// <summary>
    /// Uses basic visibility options to show and hide pages (overlays)
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        // used at the main menu to know which grid is currently in use
        Border currentGrid;

        // used for opening and closing the sqlite connection
        SQLiteConnection sqlite_conn;
        SQLiteCommand sqlite_cmd;
        SQLiteDataAdapter sqlite_data_adapter;
        SQLiteDataAdapter sqlite_data_search_adapter;

        // collections used for the combo-boxes
        private ObservableCollection<string> dinList = new ObservableCollection<string>();
        private ObservableCollection<string> pcbList = new ObservableCollection<string>();
        private ObservableCollection<string> readKeyList = new ObservableCollection<string>();

        // global variables 
        string availabilty = null;
        string searchAvail = null;
        string getProductValue;
        string getSearchValue;
        bool checkAddCombo = false;
        bool checkSearchCombo = false;
        bool checkSearchAvailClick = false;

        // data tables for the list boxes
        DataTable allDataTable = new DataTable("ICT Support");
        DataTable searchDataTable = new DataTable("User Search");

        public MainWindow()
        {
            InitializeComponent();
            AddDinValues();
            AddPCBValues();
            AddRKValues();

            // create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=ICTSupportInventory.db;Version=3;New=False;Compress=True;");

            // open the table for connection
            sqlite_conn.Open();

            // create a new SQL command:
            sqlite_cmd = sqlite_conn.CreateCommand();

            // create a new table to work with
            sqlite_cmd.CommandText = "CREATE TABLE Items (ProductType varchar(100), SerialNumber varchar(100), Location varchar(100), Availability varchar(50), Owner varchar(100), ETR varchar(100), Comments varchar(100));";

            // close the connection
            sqlite_conn.Close();

            // get values to populate all items 
            PopulateAllLists();
        }

        // adds din choices
        private void AddDinValues()
        {
            dinList.Add("");
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

        // adds pcb choices
        private void AddPCBValues()
        {
            pcbList.Add("");
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

        // adds keypad reader choices
        private void AddRKValues()
        {
            readKeyList.Add("");
            readKeyList.Add("CRX-AIP-RCV");
            readKeyList.Add("ELT-KLCD");
            readKeyList.Add("ELT-KLED");
            readKeyList.Add("ELT-KLES");
            readKeyList.Add("ELT-TLCD");
            readKeyList.Add("PIR-B1");
            readKeyList.Add("PIR-EAS");
            readKeyList.Add("PIR-EAS-PET");
            readKeyList.Add("PRT-ATH1");
            readKeyList.Add("PRT-KLCD");
            readKeyList.Add("PRT-KLCS");
            readKeyList.Add("PRT-KLES");
            readKeyList.Add("PRT-TLCD");
            readKeyList.Add("PRX-MULTI");
            readKeyList.Add("PRX-MULTI-DF");
            readKeyList.Add("PRX-MULTI-MF");
            readKeyList.Add("PRX-NPROX");
            readKeyList.Add("PRX-NPROX-DF");
            readKeyList.Add("PRX-OPIN");
            readKeyList.Add("PRX-TSEC-EXTRA");
            readKeyList.Add("PRX-TSEC-EXTRA-125-B");
            readKeyList.Add("PRX-TSEC-EXTRA-125-KP-B");
            readKeyList.Add("PRX-TSEC-EXTRA-125-KP-W");
            readKeyList.Add("PRX-TSEC-EXTRA-125-W");
            readKeyList.Add("PRX-TSEC-EXTRA-B");
            readKeyList.Add("PRX-TSEC-EXTRA-DF-B");
            readKeyList.Add("PRX-TSEC-EXTRA-DF-KP-B");
            readKeyList.Add("PRX-TSEC-EXTRA-DF-KP-W");
            readKeyList.Add("PRX-TSEC-EXTRA-DF-W");
            readKeyList.Add("PRX-TSEC-EXTRA-KP-B");
            readKeyList.Add("PRX-TSEC-EXTRA-KP-W");
            readKeyList.Add("PRX-TSEC-EXTRA-W");
            readKeyList.Add("PRX-TSEC-MINI");
            readKeyList.Add("PRX-TSEC-MINI-125-B");
            readKeyList.Add("PRX-TSEC-MINI-125-W");
            readKeyList.Add("PRX-TSEC-MINI-B");
            readKeyList.Add("PRX-TSEC-MINI-DF-B");
            readKeyList.Add("PRX-TSEC-MINI-DF-W");
            readKeyList.Add("PRX-TSEC-MINI-W");
            readKeyList.Add("PRX-TSEC-STD");
            readKeyList.Add("PRX-TSEC-STD-125-B");
            readKeyList.Add("PRX-TSEC-STD-125-KP-B");
            readKeyList.Add("PRX-TSEC-STD-125-KP-W");
            readKeyList.Add("PRX-TSEC-STD-125-W");
            readKeyList.Add("PRX-TSEC-STD-B");
            readKeyList.Add("PRX-TSEC-STD-DF-B");
            readKeyList.Add("PRX-TSEC-STD-DF-KP-B");
            readKeyList.Add("PRX-TSEC-STD-DF-KP-W");
            readKeyList.Add("PRX-TSEC-STD-DF-W");
            readKeyList.Add("PRX-TSEC-STD-KP-B");
            readKeyList.Add("PRX-TSEC-STD-KP-W");
            readKeyList.Add("PRX-TSEC-STD-W");
            readKeyList.Add("PRX-VARIO");
            readKeyList.Add("PRX-VARIO-DF");
            readKeyList.Add("PRX-VARIO-MF");
            readKeyList.Add("TFR-100-16");
            readKeyList.Add("TFR-40-16");
            readKeyList.Add("ULC-FSH-5A");
            ProductComboBox.ItemsSource = readKeyList;
        }

        // shows the all items page
        private void AllItems_Click(object sender, RoutedEventArgs e)
        {
            AllDataGrid.ItemsSource = allDataTable.DefaultView;
            currentGrid = AllItem;
            currentGrid.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Collapsed;
            HeaderLabel.Content = "All Items";
            BackButton.Visibility = Visibility.Visible;
        }

        // shows the add items page
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            SearchDataGrid.ItemsSource = allDataTable.DefaultView;
            currentGrid = AddItem;
            currentGrid.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Collapsed;
            HeaderLabel.Content = "Add a New Item";
            BackButton.Visibility = Visibility.Visible;
        }

        // shows the search items page
        private void SearchItem_Click(object sender, RoutedEventArgs e)
        {
            SearchDataGrid.ItemsSource = allDataTable.DefaultView;
            currentGrid = SearchItem;
            currentGrid.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Collapsed;
            HeaderLabel.Content = "Search Items";
            BackButton.Visibility = Visibility.Visible;
        }

        // shows the update item page
        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            currentGrid = UpdateItem;
            currentGrid.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Collapsed;
            HeaderLabel.Content = "Update an Item";
            BackButton.Visibility = Visibility.Visible;
        }

        // back button when in initial overlays
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Visible;
            currentGrid.Visibility = Visibility.Collapsed;
            HeaderLabel.Content = "Support Inventory";
            BackButton.Visibility = Visibility.Collapsed;
        }

        // when submitting a new item to the db
        private void SubmitType_Click(object sender, RoutedEventArgs e)
        {

            // check what type box was used
            if (checkAddCombo)
            {
                getProductValue = ProductComboBox.Text;
            }
            else
            {
                getProductValue = ProductTextBox.Text;
            }

            // check all fields are not null if null notify user.
            if (Check_Fields())
            {
                //CheckSQLTable();

                sqlite_conn.Open();

                sqlite_cmd.CommandText = "INSERT INTO Items (ProductType, SerialNumber, Location, Availability, Owner, ETR, Comments) VALUES ('" + getProductValue + "','" + this.SerialBox.Text + "','" + this.LocationBox.Text + "','" + availabilty + "','" + this.OwnerBox.Text + "','" + this.ETRBox.Text + "','" + this.CommentsBox.Text + "');";

                sqlite_cmd.ExecuteNonQuery();

                sqlite_conn.Close();

                ClearAll();

                MessageBox.Show("Successfully Added");
            }
            else
            {
                MessageBox.Show("Please Fill in all the Fields");
            }

        }

        // product combo box selection method (shows the users combo box in add items page)
        private void ProductType_Click(object sender, RoutedEventArgs e)
        {
            string nameOfButton = (sender as RadioButton).Name;
            if (nameOfButton.Contains("Din"))
            {
                ProductTextBox.Visibility = Visibility.Collapsed;
                ProductComboBox.ItemsSource = dinList;
                ProductComboBox.Visibility = Visibility.Visible;
                typeLabel.Content = "DIN";
                checkAddCombo = true;
            }
            else if (nameOfButton.Contains("PCB"))
            {
                ProductTextBox.Visibility = Visibility.Collapsed;
                ProductComboBox.ItemsSource = pcbList;
                ProductComboBox.Visibility = Visibility.Visible;
                typeLabel.Content = "PCB";
                checkAddCombo = true;
            }
            else if (nameOfButton.Contains("ReaderKeypad"))
            {
                ProductTextBox.Visibility = Visibility.Collapsed;
                ProductComboBox.ItemsSource = readKeyList;
                ProductComboBox.Visibility = Visibility.Visible;
                typeLabel.Content = "Reader/Keypad";
                checkAddCombo = true;
            }
            else if (nameOfButton.Contains("Other"))
            {
                ProductComboBox.Visibility = Visibility.Collapsed;
                ProductTextBox.Visibility = Visibility.Visible;
                typeLabel.Content = "Other";
                checkAddCombo = false;
            }
        }

        // product combo box selection method (shows the users combo box in search items page)
        private void SearchType_Click(object sender, RoutedEventArgs e)
        {
            string nameOfButton = (sender as RadioButton).Name;
            if (nameOfButton.Contains("Din"))
            {
                SearchProductTextBox.Visibility = Visibility.Collapsed;
                SearchProductComboBox.ItemsSource = dinList;
                SearchProductComboBox.Visibility = Visibility.Visible;
                typeLabel.Content = "DIN";
                checkSearchCombo = true;
            }
            else if (nameOfButton.Contains("PCB"))
            {
                SearchProductTextBox.Visibility = Visibility.Collapsed;
                SearchProductComboBox.ItemsSource = pcbList;
                SearchProductComboBox.Visibility = Visibility.Visible;
                typeLabel.Content = "PCB";
                checkSearchCombo = true;
            }
            else if (nameOfButton.Contains("ReaderKeypad"))
            {
                SearchProductTextBox.Visibility = Visibility.Collapsed;
                SearchProductComboBox.ItemsSource = readKeyList;
                SearchProductComboBox.Visibility = Visibility.Visible;
                typeLabel.Content = "Reader/Keypad";
                checkSearchCombo = true;
            }
            else if (nameOfButton.Contains("Other"))
            {
                SearchProductComboBox.Visibility = Visibility.Collapsed;
                SearchProductTextBox.Visibility = Visibility.Visible;
                typeLabel.Content = "Other";
                checkSearchCombo = false;
            }
        }

        // checks the validity of each entry box in the add items page
        private bool Check_Fields()
        {
            bool nullValues = true;

            if (getProductValue.Length == 0)
            {
                nullValues = false;
            }
            if (SerialBox.Text.Length == 0)
            {
                nullValues = false;
            }
            if (LocationBox.Text.Length == 0)
            {
                nullValues = false;
            }
            if (availabilty.Length == 0)
            {
                nullValues = false;
            }
            if (availabilty == "No")
            {
                if (ETRBox.Text.Length == 0)
                {
                    nullValues = false;
                }
            }
            if (OwnerBox.Text.Length == 0)
            {
                nullValues = false;
            }
            return nullValues;
        }

        // checks the radio buttons for availability (whether checked or not)
        private void Check_Click(object sender, RoutedEventArgs e)
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

        // clears all fields in the add items page (calls clearall)
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        // clear all explicitly tells all boxes to be ""
        private void ClearAll()
        {
            ETRBox.Text = "";
            availabilty = "";
            OwnerBox.Text = "";
            LocationBox.Text = "";
            SerialBox.Text = "";
            CommentsBox.Text = "";
            ProductTextBox.Text = "";
            typeLabel.Content = "Product Type?";
            checkYes.IsChecked = false;
            checkNo.IsChecked = false;
            Din.IsChecked = false;
            PCB.IsChecked = false;
            ReaderKeypad.IsChecked = false;
            Other.IsChecked = false;
            ProductTextBox.Visibility = Visibility.Collapsed;
            ProductComboBox.Visibility = Visibility.Collapsed;
            dueBackLabel.Visibility = Visibility.Collapsed;
            DuebackPanel.Visibility = Visibility.Collapsed;
        }

        // alters the header label from within the search menu
        private void SearchAvailable_Click(object sender, RoutedEventArgs e)
        {
            string val = (sender as RadioButton).Name;
            if (val.Contains("SearchAvailable"))
            {
                HeaderLabel.Content = "Available Items";
                searchAvail = "Yes";
                checkSearchAvailClick = true;
            }
            else
            {
                HeaderLabel.Content = "Unavailable Items";
                searchAvail = "No";
                checkSearchAvailClick = true;
            }
        }

        // populates the all items page with values from the database
        private void PopulateAllLists()
        {
            //CheckSQLTable();
            allDataTable.Clear();

            sqlite_conn.Open();

            sqlite_cmd.CommandText = "SELECT ProductType, SerialNumber, Location, Availability, Owner, ETR, Comments FROM Items";

            sqlite_data_adapter = new SQLiteDataAdapter(sqlite_cmd);

            sqlite_data_adapter.Fill(allDataTable);

            sqlite_conn.Close();
        }

        // refresh all calls the populate lists method to refresh any new added items
        private void RefreshAll_Click(object sender, RoutedEventArgs e)
        {
            PopulateAllLists();
        }
        
        // filters the input boxes for the items required
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkSearchCombo)
            {
                getSearchValue = SearchProductComboBox.Text;
            }
            else
            {
                getSearchValue = SearchProductTextBox.Text;
            }

            string tempVal = "SELECT * FROM Items";

            if (getSearchValue.Length != 0)
            {
                tempVal += " WHERE ProductType='" + getSearchValue + "'";
                if (SerialNum.Text.Length != 0)
                {
                    tempVal += " AND SerialNumber='" + this.SerialNum.Text + "'";
                }

                if (checkSearchAvailClick)
                {
                    tempVal += " AND Availability='" + searchAvail + "'";
                }
                //MessageBox.Show(tempVal);
                FillData(tempVal);

            }
            else if (SerialNum.Text.Length != 0)
            {
                tempVal += " WHERE SerialNumber='" + this.SerialNum.Text + "'";
                if (checkSearchAvailClick)
                {
                    tempVal += " AND Availability='" + searchAvail + "'";
                }
                //MessageBox.Show(tempVal);
                FillData(tempVal);
            }
            else if (checkSearchAvailClick)
            {
                tempVal += " WHERE Availability='" + searchAvail + "'";
                //MessageBox.Show(tempVal);
                FillData(tempVal);
            }
            else
            {
                MessageBox.Show("Please fill in atleast one of the fields");
            }
        }
        
        // fills the data from the database into a DataTable and loads it into the searchgrid
        private void FillData(string val)
        {
            searchDataTable.Clear();

            sqlite_conn.Open();

            sqlite_cmd.CommandText = val;

            sqlite_data_search_adapter = new SQLiteDataAdapter(sqlite_cmd);

            sqlite_data_search_adapter.Fill(searchDataTable);

            SearchDataGrid.ItemsSource = searchDataTable.DefaultView;

            sqlite_conn.Close();
        }

        // clears the entries the user made
        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            HeaderLabel.Content = "Search Items";
            checkSearchAvailClick = false;
            searchAvail = "";
            SearchAvailable.IsChecked = false;
            SearchUnavailable.IsChecked = false;
            SearchProductComboBox.Text = "";
            SearchProductTextBox.Text = "";
            SerialNum.Text = "";
            SearchProductTextBox.Visibility = Visibility.Collapsed;
            SearchProductComboBox.Visibility = Visibility.Collapsed;
        }

        private void AllDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Delete this Item?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff, aka do nothing
            }
            else
            {
                MessageBox.Show("Delete not yet available");
            }
        }


    }
}



// meant to check the validity of the table in the db (however does not work)
//private void CheckSQLTable()
//{
//    sqlite_conn.Open();

//    sqlite_cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Items';";

//    sqlite_cmd.ExecuteNonQuery();

//    sqlite_conn.Close();
//}

//

//