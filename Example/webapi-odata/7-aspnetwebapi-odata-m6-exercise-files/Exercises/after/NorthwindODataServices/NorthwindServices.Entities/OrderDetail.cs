using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NorthwindServices.Entities
{
    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        //public Product Product { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public Single Discount { get; set; }
    }
}
