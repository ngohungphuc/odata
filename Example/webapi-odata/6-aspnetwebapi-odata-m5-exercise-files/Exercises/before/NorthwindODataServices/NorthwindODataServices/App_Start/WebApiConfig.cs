using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using Microsoft.Data.Edm;
using NorthwindServices.Entities;

namespace NorthwindODataServices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapODataRoute("Northwind", "odata", GetImplicitEDM());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static IEdmModel GetImplicitEDM()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Order>("Orders");
            return builder.GetEdmModel(); // magic happens here
        }

        private static IEdmModel GetExplicitEDM()
        {
            ODataModelBuilder builder = new ODataModelBuilder();
            var customers = builder.EntitySet<Customer>("Customers");
            var orders = builder.EntitySet<Order>("Orders");
            var customer = customers.EntityType;
            customer.HasKey(c => c.CustomerID);
            customer.Property(c => c.CompanyName);
            customer.Property(c => c.Phone);
            customer.Property(c => c.ContactName);
            customer.HasMany(c => c.Orders);

            var order = orders.EntityType;
            order.HasKey(o => o.OrderID);
            // etc
            return builder.GetEdmModel();
        }
    }
}
