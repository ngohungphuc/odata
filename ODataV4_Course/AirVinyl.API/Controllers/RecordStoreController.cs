using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using AirVinyl.API.Helpers;
using AirVinyl.DataAccessLayer;
using AirVinyl.Model;

namespace AirVinyl.API.Controllers
{
    public class RecordStoresController : ODataController
    {
        // context
        private readonly AirVinylDbContext _context = new AirVinylDbContext();

        // GET odata/RecordStores
        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_context.RecordStores);
        }

        // GET odata/RecordStores(key)
        [EnableQuery]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            var recordStores = _context.RecordStores.Where(p => p.RecordStoreId == key);

            if (!recordStores.Any())
            {
                return NotFound();
            }

            return Ok(SingleResult.Create(recordStores));
        }

        [HttpGet]
        [ODataRoute("RecordStores({key})/Tags")]
        [EnableQuery]
        public IHttpActionResult GetRecordStoreTagsProperty([FromODataUri] int key)
        {
            // no Include necessary for EF - Tags isn't a navigation property 
            // in the entity model.  
            var recordStore = _context.RecordStores
                .FirstOrDefault(p => p.RecordStoreId == key);

            if (recordStore == null)
            {
                return NotFound();
            }

            var collectionPropertyToGet = Url.Request.RequestUri.Segments.Last();
            var collectionPropertyValue = recordStore.GetValue(collectionPropertyToGet);

            // return the collection of tags
            return this.CreateOKHttpActionResult(collectionPropertyValue);
        }

        [HttpPost]
        [ODataRoute("RecordStores")]
        public IHttpActionResult CreateRecordStore(RecordStore recordStore)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // add the RecordStore
            _context.RecordStores.Add(recordStore);
            _context.SaveChanges();

            // return the created RecordStore 
            return Created(recordStore);
        }

        [HttpPatch]
        [ODataRoute("RecordStores({key})")]
        [ODataRoute("RecordStores({key})/AirVinyl.Model.SpecializedRecordStore")]
        public IHttpActionResult UpdateRecordStorePartially([FromODataUri] int key, Delta<RecordStore> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // find a matching record store
            var currentRecordStore = _context.RecordStores.FirstOrDefault(p => p.RecordStoreId == key);

            // if the record store isn't found, return NotFound
            if (currentRecordStore == null)
            {
                return NotFound();
            }

            patch.Patch(currentRecordStore);
            _context.SaveChanges();

            // return NoContent
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [ODataRoute("RecordStores({key})")]
        [ODataRoute("RecordStores({key})/AirVinyl.Model.SpecializedRecordStore")]
        public IHttpActionResult DeleteRecordStore([FromODataUri] int key)
        {
            var currentRecordStore = _context.RecordStores.Include("Ratings")
                .FirstOrDefault(p => p.RecordStoreId == key);
            if (currentRecordStore == null)
            {
                return NotFound();
            }

            currentRecordStore.Ratings.Clear();
            _context.RecordStores.Remove(currentRecordStore);
            _context.SaveChanges();

            // return NoContent
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [ODataRoute("RecordStores({key})/AirVinyl.Functions.IsHighRated(minimumRating={minimumRating})")]
        public bool IsHighRated([FromODataUri] int key, int minimumRating)
        {
            var recordStore = _context.RecordStores
                .FirstOrDefault(p => p.RecordStoreId == key && p.Ratings.Any()
                && (p.Ratings.Sum(r => r.Value) / p.Ratings.Count) >= minimumRating);
            return (recordStore != null);
        }

        [HttpGet]
        [ODataRoute("RecordStores/AirVinyl.Functions.AreRatedBy(personIds={personIds})")]
        public IHttpActionResult AreRatedBy([FromODataUri] IEnumerable<int> personIds)
        {
            // get the RecordStores
            var recordStores = _context.RecordStores
                .Where(p => p.Ratings.Any(r => personIds.Contains(r.RatedBy.PersonId)));

            return this.CreateOKHttpActionResult(recordStores);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}