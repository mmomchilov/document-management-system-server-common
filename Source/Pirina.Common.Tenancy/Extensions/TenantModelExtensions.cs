using System;
using Glasswall.Common.TenancyHelpers;
using Glasswall.Kernel.Data.Tenancy;

namespace Glasswall.Common.Tenancy.Extensions
{
    public static class TenantModelExtensions
    {
        /// <summary>
        /// Assign tenantId to a tenant model
        /// </summary>
        /// <typeparam name="T">Tenant model type</typeparam>
        /// <param name="item">Tenant model</param>
        /// <param name="tenantId">Tenant identifier</param>
        /// <returns></returns>
        public static T AssignTenantId<T>(this T item, Guid tenantId) where T : BaseTenantModel
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var propertyDelegate = QueryHelper.GetAssignDelegate<T>(t => t.TenantId);
            propertyDelegate(item, tenantId);
            return item;
        }
    }
}