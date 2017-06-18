using NorthwindServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            NorthwindDbContext context = new NorthwindDbContext();
            context.Configuration.LazyLoadingEnabled = false;
            var customers = (from c in context.Customers select c).ToList();
            var customersWithOrders = (from c in context.Customers.Include("Orders") select c).ToList();
            var orders = (from o in context.Orders select o).ToList();
            var orderDetails = (from od in context.OrderDetails select od).ToList();
            var employees = (from e in context.Employees select e).ToList();
        }
    }
}
