using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using Microsoft.Data.OData;
using NorthwindServices.Data;
using NorthwindServices.Entities;
using System.Net.Http;
using System.Net;

namespace NorthwindODataServices.Controllers
{
    public class CustomersController : EntitySetController<Customer,string>
    {
        NorthwindDbContext _Context = new NorthwindDbContext();

        public CustomersController()
        {
            _Context.Configuration.LazyLoadingEnabled = false;
        }

        public override IQueryable<Customer> Get()
        {
            return _Context.Customers;
        }

        protected override Customer GetEntityByKey(string key)
        {
            return _Context.Customers.FirstOrDefault(c => c.CustomerID == key);
        }

        public IQueryable<Order> GetOrdersFromCustomer([FromODataUri] string key)
        {
            return _Context.Orders.Where(o => o.CustomerID == key);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _Context.Dispose();
        }

    }
}
