using System;
using System.Linq;
using Glasswall.Common.TenancyHelpers;
using Glasswall.Kernel.Data.Tenancy;

namespace Glasswall.Common.Tenancy.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Add where clause with TenantId==tenantId predicate to query 
        /// </summary>
        /// <typeparam name="T">Tenant model</typeparam>
        /// <param name="query">query to filter</param>
        /// <param name="tenantId">Tenant identifier</param>
        /// <returns></returns>
        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, Guid tenantId) where T : BaseTenantModel
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            return QueryHelper.BuildFilterQuery(query, x => x.TenantId, tenantId);
        }
    }
}