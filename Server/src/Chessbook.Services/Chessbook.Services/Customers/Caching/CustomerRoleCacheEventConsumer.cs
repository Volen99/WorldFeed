﻿using System.Threading.Tasks;

using Chessbook.Core.Domain.Customers;
using Chessbook.Services.Caching;

namespace Chessbook.Services.Customers.Caching
{
    /// <summary>
    /// Represents a customer role cache event consumer
    /// </summary>
    public partial class CustomerRoleCacheEventConsumer : CacheEventConsumer<CustomerRole>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(CustomerRole entity)
        {
            await RemoveByPrefixAsync(NopCustomerServicesDefaults.CustomerRolesBySystemNamePrefix);
            await RemoveByPrefixAsync(NopCustomerServicesDefaults.CustomerCustomerRolesPrefix);
        }
    }
}
