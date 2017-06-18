//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel.Web;
using System.Web;

namespace NorthwindWCFDataServicesDemo
{
    public class NorthwindDataService : DataService< NorthwindEntities >
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            config.SetEntitySetAccessRule("Customers", EntitySetRights.All);
            config.SetEntitySetAccessRule("Orders", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Order_Details", EntitySetRights.AllRead);
            config.SetServiceOperationAccessRule("OrderedCustomers", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }

        [WebGet(UriTemplate = "OrderedCustomers")]
        public IQueryable<Customer> OrderedCustomers()
        {
            return this.CurrentDataSource.Customers.OrderBy(c => c.CompanyName);
        }


        [QueryInterceptor("Customers")]
        public Expression<Func<Customer, bool>> OnQueryCustomers()
        {
            return c => c.Country == "USA";
        }

        [ChangeInterceptor("Customers")]
        public void UpdateCustomer(Customer c, UpdateOperations ops)
        {
            if (string.IsNullOrWhiteSpace(c.CompanyName))
                throw new DataServiceException("Company name is required");
        }

    }
}
