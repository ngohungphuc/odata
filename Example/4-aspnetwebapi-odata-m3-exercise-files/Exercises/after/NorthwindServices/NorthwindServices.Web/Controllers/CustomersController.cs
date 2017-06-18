using NorthwindServices.Data;
using NorthwindServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NorthwindServices.Web.Controllers
{
    public class CustomersController : ApiController
    {
        NorthwindDbContext _Context = new NorthwindDbContext();

        public CustomersController()
        {
            _Context.Configuration.LazyLoadingEnabled = false;
        }

        // GET /api/Customers
        [Queryable]
        public IQueryable<Customer> GetCustomers()
        {
            return _Context.Customers;
        }

        // GET /api/Customers/ALFKI
        public Customer GetCustomer(string id)
        {
            Customer customer = _Context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return customer;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _Context.Dispose();
        }

    }
}
