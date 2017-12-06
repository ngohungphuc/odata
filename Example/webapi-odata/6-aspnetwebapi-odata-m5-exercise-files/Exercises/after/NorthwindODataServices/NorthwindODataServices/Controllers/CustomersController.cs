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

        protected override Customer CreateEntity(Customer entity)
        {
            _Context.Customers.Add(entity);
            _Context.SaveChanges();
            return entity;
        }

        protected override string GetKey(Customer entity)
        {
            return entity.CustomerID;
        }

        protected override Customer UpdateEntity(string key, Customer update)
        {
            if (!_Context.Customers.Any(c => c.CustomerID == key))
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = "Customer key " + key + " not found"
                    }));
            }
            update.CustomerID = key; // ignore the ID in the entity use the ID in the URL.

            _Context.Customers.Attach(update);
            _Context.Entry(update).State = System.Data.EntityState.Modified;
            _Context.SaveChanges();
            return update;
        }

        public override void Delete([FromODataUri] string key)
        {
            var customer = _Context.Customers.FirstOrDefault(c => c.CustomerID == key);
            if (customer == null)
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = "Customer key " + key + " not found"
                    }));
            }
            _Context.Customers.Remove(customer);
            _Context.SaveChanges();
        }

        protected override Customer PatchEntity(string key, Delta<Customer> patch)
        {
            var customer = _Context.Customers.FirstOrDefault(c => c.CustomerID == key);
            if (customer == null)
            {
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(
                    HttpStatusCode.NotFound,
                    new ODataError
                    {
                        ErrorCode = "EntityNotFound",
                        Message = "Customer key " + key + " not found"
                    }));
            }
            patch.Patch(customer);
            _Context.SaveChanges();
            return customer;
        }

        [HttpPost]
        public int AddLoyaltyPoints([FromODataUri] string key, ODataActionParameters parms)
        {
            string category = parms["category"] as string;
            int points = (int)parms["points"];
            // Pretend
            int currentPoints = 42; // look up based on category
            return currentPoints + points;
        }

        public override HttpResponseMessage HandleUnmappedRequest(System.Web.Http.OData.Routing.ODataPath odataPath)
        {
            return base.HandleUnmappedRequest(odataPath);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _Context.Dispose();
        }

    }
}
