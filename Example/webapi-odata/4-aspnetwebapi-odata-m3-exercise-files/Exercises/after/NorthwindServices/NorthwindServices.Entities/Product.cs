using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NorthwindServices.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
