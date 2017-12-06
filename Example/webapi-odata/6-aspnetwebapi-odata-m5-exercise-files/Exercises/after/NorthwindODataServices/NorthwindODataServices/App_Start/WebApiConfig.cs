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
            var customers = builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Order>("Orders");

            var customer = customers.EntityType;
            var action = customer.Action("AddLoyaltyPoints");
            action.Parameter<string>("category");
            action.Parameter<int>("points");
            action.Returns<int>();

            return builder.GetEdmModel(); // magic happens here
        }
    }
}
