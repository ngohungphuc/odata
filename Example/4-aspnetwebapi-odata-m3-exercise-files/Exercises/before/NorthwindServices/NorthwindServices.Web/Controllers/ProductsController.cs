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
    public class ProductsController : ApiController
    {
        NorthwindDbContext _Context = new NorthwindDbContext();
        [HttpGet]
        //[Queryable]
        public IQueryable<Product> GimmeSomeProducts()
        {
            return _Context.Products;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _Context.Dispose();
        }
    }
}
