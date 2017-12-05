using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using AirVinyl.Model;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace AirVinyl.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapODataServiceRoute("OdataRoute", "odata", GetEdmModel(config));
            config.EnsureInitialized();
        }

        private static IEdmModel GetEdmModel(HttpConfiguration config)
        {
            var builder = new ODataConventionModelBuilder
            {
                Namespace = "AirVinyl",
                ContainerName = "AirVinylContainer"
            };

            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
            builder.EntitySet<Person>("People");
            builder.EntitySet<VinylRecord>("VinylRecords");

            return builder.GetEdmModel();
        }
    }
}
