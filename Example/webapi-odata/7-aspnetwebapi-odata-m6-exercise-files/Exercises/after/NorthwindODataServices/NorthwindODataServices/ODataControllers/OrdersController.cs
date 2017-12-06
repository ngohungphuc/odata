using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using NorthwindServices.Data;
using NorthwindServices.Entities;
using System.Net.Http;
using System.Net;
using Microsoft.Data.OData;

namespace NorthwindODataServices.Controllers
{
    public class OrdersController : EntitySetController<Order, int>
    {
        NorthwindDbContext _Context = new NorthwindDbContext();
        public OrdersController()
        {
            _Context.Configuration.LazyLoadingEnabled = false;
        }
        public override IQueryable<Order> Get()
        {
            return _Context.Orders;
        }

        protected override Order GetEntityByKey(int key)
        {
            return _Context.Orders.FirstOrDefault(o => o.OrderID == key);
        }

        protected override int GetKey(Order entity)
        {
            return entity.OrderID;
        }

        protected override Order CreateEntity(Order entity)
        {
            _Context.Orders.Add(entity);
            _Context.SaveChanges();
            return entity;
        }

        public override void Delete([FromODataUri] int key)
        {
            var order = _Context.Orders.FirstOrDefault(o => o.OrderID == key);
            if (order == null) 
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(HttpStatusCode.NotFound, 
                    new ODataError{Message = "Order " + key + " not found"}));
            _Context.Orders.Remove(order);
            _Context.SaveChanges();
        }

        protected override Order UpdateEntity(int key, Order update)
        {
            if (!_Context.Orders.Any(o => o.OrderID == key)) 
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(HttpStatusCode.NotFound, 
                    new ODataError { Message = "Order " + key + " not found" }));
            update.OrderID = key;
            _Context.Orders.Attach(update);
            _Context.Entry(update).State = System.Data.EntityState.Modified;
            _Context.SaveChanges();
            return update;
        }

        protected override Order PatchEntity(int key, Delta<Order> patch)
        {
            var order = _Context.Orders.FirstOrDefault(o => o.OrderID == key);
            if (order == null) 
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(HttpStatusCode.NotFound, 
                    new ODataError{Message = "Order " + key + " not found"}));
            patch.Patch(order);
            _Context.SaveChanges();
            return order;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _Context.Dispose();
        }
    }
}
