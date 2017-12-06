using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using AirVinyl.Model;
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
            builder.EntitySet<RecordStore>("RecordStores");

            // function bound to RecordStore entity
            var isHighRatedFunction = builder.EntityType<RecordStore>().Function("IsHighRated");
            isHighRatedFunction.Returns<bool>();
            isHighRatedFunction.Parameter<int>("minimumRating");
            isHighRatedFunction.Namespace = "AirVinyl.Functions";

            // function bound to RecordStore list
            var areRatedByFunction = builder.EntityType<RecordStore>().Collection
                .Function("AreRatedBy");

            areRatedByFunction.ReturnsCollectionFromEntitySet<RecordStore>("RecordStores");
            areRatedByFunction.CollectionParameter<int>("personIds");
            areRatedByFunction.Namespace = "AirVinyl.Functions";

            return builder.GetEdmModel();
        }
    }

    internal class ReplaceNullContentWithNotFoundAttribute : EnableQueryAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            HttpResponseMessage httpResponseMessage = actionExecutedContext.Response;
            if (httpResponseMessage.IsSuccessStatusCode && IsContentMissingValue(httpResponseMessage))
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        private static bool IsContentMissingValue(HttpResponseMessage httpResponseMessage)
        {
            if (!(httpResponseMessage.Content is ObjectContent objectContent))
            {
                return false;
            }

            var type = GetType(objectContent);
            return type == typeof(SingleResult<>) && objectContent.Value == null;
        }

        private static Type GetType(ObjectContent objectContent)
        {
            var type = objectContent.ObjectType;
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                type = genericTypeDefinition;
            }
            return type;
        }
    }
}
