using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Glasswall.Kernel.CQRS.Projections;
using Glasswall.Kernel.Data.ORM;

namespace Glasswall.Common.CQRS.Projections
{
    public class Projector<TModel> : IProjector<TModel> where TModel : class
    {
        protected readonly IDbContext Context;

        public Projector(IDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public virtual IQueryable<TModel> GetAll()
        {
            return Context.Set<TModel>();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Context?.Dispose();
        }
    }
}