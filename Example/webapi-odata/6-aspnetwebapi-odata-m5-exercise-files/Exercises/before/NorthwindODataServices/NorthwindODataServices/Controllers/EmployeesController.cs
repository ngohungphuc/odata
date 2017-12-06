using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using Microsoft.Data.OData;
using NorthwindServices.Data;
using NorthwindServices.Entities;

namespace NorthwindODataServices.Controllers
{
    public class EmployeesController : EntitySetController<Employee,int>
    {
        NorthwindDbContext _Context = new NorthwindDbContext();
        public override IQueryable<Employee> Get()
        {
            return _Context.Employees;
        }

        protected override Employee GetEntityByKey(int key)
        {
            var employee = _Context.Employees.FirstOrDefault(e => e.EmployeeID == key);
            return employee;
        }

        protected override int GetKey(Employee entity)
        {
            return entity.EmployeeID;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _Context.Dispose();
        }
    }
}
