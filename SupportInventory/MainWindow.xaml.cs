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

namespace SupportInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AllItems_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            //AddItems.Visibility = Visibility.Visible;
        }

        private void Available_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Unavailable_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
