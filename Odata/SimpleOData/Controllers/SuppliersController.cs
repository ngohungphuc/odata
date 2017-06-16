using Odata.Models;
using SimpleOData.Models;
using System.Linq;
using System.Web.Http;
using System.Web.OData;

namespace SimpleOData.Controllers
{
    public class SuppliersController : ODataController
    {
        private ODataContext db = new ODataContext();

        // GET /Products(1)/Supplier
        [EnableQuery]
        public SingleResult<Supplier> GetSupplier([FromODataUri] int key)
        {
            var result = db.Products.Where(m => m.Id == key).Select(m => m.Supplier);
            return SingleResult.Create(result);
        }

        // GET /Suppliers(1)/Products
        [EnableQuery]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.Suppliers.Where(m => m.Id.Equals(key)).SelectMany(m => m.Products);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}