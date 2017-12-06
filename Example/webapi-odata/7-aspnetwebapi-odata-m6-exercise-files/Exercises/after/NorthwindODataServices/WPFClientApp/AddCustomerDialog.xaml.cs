using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPFClientApp.NorthwindODataServices;

namespace WPFClientApp
{
    /// <summary>
    /// Interaction logic for AddCustomerDialog.xaml
    /// </summary>
    public partial class AddCustomerDialog : Window
    {
       
        public AddCustomerDialog()
        {
            Customer = new Customer();
            InitializeComponent();
            DataContext = Customer;
        }

        public Customer Customer { get; set; }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
