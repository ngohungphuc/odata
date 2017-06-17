using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using WebAPIODataV4.Models;

namespace WebAPIODataV4.Controllers
{
    public class PersonController : ODataController
    {
        private readonly DomainModel _db = new DomainModel();

        [EnableQuery(PageSize = 20)]
        public IHttpActionResult Get()
        {
            return Ok(_db.Person.AsQueryable());
        }

        public IHttpActionResult Get([FromODataUri] int key)
        {
            return Ok(_db.Person.SingleOrDefault(t => t.BusinessEntityID == key));
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}