using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using AirVinyl.DataAccessLayer;

namespace AirVinyl.API.Controllers
{
    public class PeopleController: ODataController
    {
        private AirVinylDbContext _context = new AirVinylDbContext();

        public IHttpActionResult Get()
        {
            return Ok(_context.People);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}