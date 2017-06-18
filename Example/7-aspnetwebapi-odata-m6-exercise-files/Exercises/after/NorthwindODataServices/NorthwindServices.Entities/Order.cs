using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NorthwindServices.Entities
{
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        //public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
