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
        public IEnumerable<Customer> GetCustomers()
        {
            return _Context.Customers.ToArray();
        }

        // GET /api/Customers/ALFKI
        public Customer GetCustomer(string id)
        {
            Customer customer = _Context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return customer;
        }

        // POST /api/Customers
        public HttpResponseMessage PostCustomer(Customer customer)
        {
            var matchingCustomerId = _Context.Customers.Where(c => c.CustomerID == customer.CustomerID).Select(c => c.CustomerID).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(matchingCustomerId))
                throw new HttpResponseException(HttpStatusCode.Forbidden);

            _Context.Customers.Add(customer);
            _Context.SaveChanges();
            var response = Request.CreateResponse<Customer>(HttpStatusCode.Created, customer);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = customer.CustomerID }));
            return response;

        }

        // PUT /api/Customer/ALFKI
        public Customer PutCustomer(string id, Customer customer)
        {
            var matchingCustomer = _Context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (matchingCustomer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            SetChangedProperties(matchingCustomer, customer);
            _Context.SaveChanges();
            return matchingCustomer;
        }

        // DELETE /api/northwind/ALFKI
        public void DeleteCustomer(string id)
        {
            var matchingCustomer = _Context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (matchingCustomer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _Context.Customers.Remove(matchingCustomer);
            _Context.SaveChanges();
        }


        static void SetChangedProperties(object target, object source)
        {
            if (target.GetType() != source.GetType()) throw new ArgumentException("Unmatching types");
            var props = target.GetType().GetProperties();
            foreach (var prop in props)
            {
                object valTarget = prop.GetValue(target, null);
                object valSource = prop.GetValue(source, null);
                if (valTarget != null && !valTarget.Equals(valSource)) prop.SetValue(target, valSource, null);
                else prop.SetValue(target, valSource, null);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _Context.Dispose();
        }

    }
}
