using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Windows;
using WPFClientApp.NorthwindDataServices;

namespace WPFClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NorthwindEntities _Proxy;
        DataServiceCollection<Customer> _Customers = new DataServiceCollection<Customer>();
        public MainWindow()
        {
            InitializeComponent();
            _Proxy = new NorthwindEntities(new Uri("http://localhost.:2112/NorthwindDataService.svc"));
        }

        private void OnQuery(object sender, RoutedEventArgs e)
        {
            _Customers.Load(_Proxy.Customers);
            ResultsGrid.ItemsSource = _Customers;
        }


        private void OnSave(object sender, RoutedEventArgs e)
        {
            _Proxy.SaveChanges();

        }
    }
}
