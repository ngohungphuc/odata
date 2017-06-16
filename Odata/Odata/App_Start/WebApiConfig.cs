using Odata.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace Odata
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Odata register
            //Creates an Entity Data Model (EDM).
            //Adds a route.
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("products");
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: builder.GetEdmModel()
            );

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}