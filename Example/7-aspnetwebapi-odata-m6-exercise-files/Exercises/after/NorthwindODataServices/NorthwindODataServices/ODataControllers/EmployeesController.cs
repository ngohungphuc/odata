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
            if (employee == null)
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(HttpStatusCode.NotFound,
                    new ODataError { Message = "Employee ID " + key + " not found"}));
            return employee;
        }

        protected override int GetKey(Employee entity)
        {
            return entity.EmployeeID;
        }

        public override void CreateLink(int key, string navigationProperty, Uri link)
        {
            if (navigationProperty != "PhotoPath")
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(HttpStatusCode.NotFound,
                    new ODataError { Message = "Navigation property " + navigationProperty + " does not exist on Employee type" }));
            var employee = _Context.Employees.FirstOrDefault(e => e.EmployeeID == key);
            if (employee == null)
                throw new HttpResponseException(
                    Request.CreateODataErrorResponse(HttpStatusCode.NotFound,
                    new ODataError { Message = "Employee ID " + key + " not found" }));
            employee.PhotoPath = link.ToString();
            _Context.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _Context.Dispose();
        }
    }
}
