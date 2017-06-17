using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using ODataMovies.Models;

namespace ODataMovies
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Movie>("Movies");
            config.MapODataServiceRoute("Movies", "odata", modelBuilder.GetEdmModel());
        }
    }
}