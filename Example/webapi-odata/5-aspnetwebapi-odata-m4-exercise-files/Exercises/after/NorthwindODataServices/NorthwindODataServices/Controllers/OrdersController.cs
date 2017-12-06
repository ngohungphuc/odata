using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.OData;
using NorthwindServices.Data;
using NorthwindServices.Entities;

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
            return _Context.Orders ;
        }

        protected override Order GetEntityByKey(int key)
        {
            return _Context.Orders.FirstOrDefault(o=>o.OrderID == key);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _Context.Dispose();
        }
    }
}
