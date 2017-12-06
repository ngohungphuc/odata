using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFClientApp.NorthwindODataServices;

namespace WPFClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Container _Proxy;
        DataServiceCollection<Customer> _Customers = new DataServiceCollection<Customer>();

        public MainWindow()
        {
            InitializeComponent();
            _Proxy = new Container(new Uri("http://localhost.:2112/odata/"));

        }

        private async void OnQuery(object sender, RoutedEventArgs e)
        {
            IQueryable<Customer> query = from c in _Proxy.Customers where c.Country == "Germany" select c;
            var customers = await GetCustomersAsync(query);
            _Customers.Load(customers);
            ResultsGrid.ItemsSource = _Customers;
        }

        Task<IEnumerable<Customer>> GetCustomersAsync(IQueryable<Customer> query)
        {
            DataServiceQuery<Customer> dsQuery = (DataServiceQuery<Customer>)(query);
            return Task.Factory.FromAsync<IEnumerable<Customer>>(dsQuery.BeginExecute, dsQuery.EndExecute, null);
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            _Proxy.SaveChanges();
        }

        private void OnCreate(object sender, RoutedEventArgs e)
        {
            AddCustomerDialog dialog = new AddCustomerDialog();
            bool? result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                _Customers.Add(dialog.Customer);
                _Proxy.SaveChanges();
            }
        }

        private async void OnDelete(object sender, RoutedEventArgs e)
        {
            _Customers.Remove(ResultsGrid.SelectedItem as Customer);
            await SaveAsync();
        }

        Task<DataServiceResponse> SaveAsync()
        {
            return Task.Factory.FromAsync<DataServiceResponse>(_Proxy.BeginSaveChanges, _Proxy.EndSaveChanges, null);
        }

    }
}
