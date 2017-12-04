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
    public class VinylRecordsController: ODataController
    {
        private AirVinylDbContext _context = new AirVinylDbContext();

        [ODataRoute("VinylRecords")]
        public IHttpActionResult Get()
        {
            return Ok(_context.VinylRecords);
        }

        [ODataRoute("VinylRecords({key})")]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            var vinyl = _context.VinylRecords.FirstOrDefault(p => p.VinylRecordId == key);
            if (vinyl == null)
            {
                return NotFound();
            }
            return Ok(vinyl);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}