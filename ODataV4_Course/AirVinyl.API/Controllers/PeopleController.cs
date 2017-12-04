using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using AirVinyl.DataAccessLayer;

namespace AirVinyl.API.Controllers
{
    public class PeopleController : ODataController
    {
        private AirVinylDbContext _context = new AirVinylDbContext();

        public IHttpActionResult Get()
        {
            return Ok(_context.People);
        }

        [ODataRoute("People({key})")]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            var person = _context.People.FirstOrDefault(p => p.PersonId == key);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}