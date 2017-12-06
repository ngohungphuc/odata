using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using NorthwindServices.Data;
using NorthwindServices.Entities;

namespace NorthwindODataServices.Controllers
{
    public class OrdersController : EntitySetController<Order,int>
    {
        NorthwindDbContext _Context = new NorthwindDbContext();

        [Queryable]
        public override IQueryable<Order> Get()
        {
            return _Context.Orders;
        }

        public override void CreateLink([FromODataUri] int key, string navigationProperty, [FromBody] Uri link)
        {
            // Use link URL to locate appropriate customer and update the data storage
        }
    }
}
