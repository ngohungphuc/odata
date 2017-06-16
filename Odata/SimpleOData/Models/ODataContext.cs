using SimpleOData.Models;
using System.Data.Entity;

namespace Odata.Models
{
    public class ODataContext : DbContext
    {
        public ODataContext() : base("name=ODataContext")
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}