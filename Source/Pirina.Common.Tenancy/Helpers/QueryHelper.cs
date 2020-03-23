using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using Glasswall.Kernel.Reflection.Extensions;
using Glasswall.Kernel.Reflection.Reflection;

namespace Glasswall.Common.TenancyHelpers
{
    internal class QueryHelper
    {
        private static ConcurrentDictionary<Type, Action<object, object>> _delegateCache = new ConcurrentDictionary<Type, Action<object, object>>();
        internal static IQueryable<T> BuildFilterQuery<T>(IQueryable<T> query, Expression<Func<T, object>> property, Guid id)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            var left = QueryHelper.GetMemberExpression(property);
            var pe = (ParameterExpression)left.Expression;
            var right = Expression.Constant(id, typeof(Guid));
            var predicateBody = Expression.Equal(left, right);

            var whereCallExpression = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Where),
                new Type[] { query.ElementType },
                query.Expression,
                Expression.Lambda<Func<T, bool>>(predicateBody, new ParameterExpression[] { pe }));
            return query.Provider.CreateQuery<T>(whereCallExpression);
        }

        internal static Action<object, object> GetAssignDelegate<T>(Expression<Func<T, object>> property)
        {
            var pinfo = ReflectionHelper.GetProperty(property);
            var del = QueryHelper._delegateCache.GetOrAdd(typeof(T), t => TypeExtensions.GetAssignPropertyDelegate(t, pinfo.Name));
            return del;
        }

        internal static MemberExpression GetMemberExpression(LambdaExpression expression)
        {
            MemberExpression memberExpr = null;

            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                    memberExpr = ((UnaryExpression)expression.Body).Operand as MemberExpression;
                    break;
                case ExpressionType.MemberAccess:
                    memberExpr = expression.Body as MemberExpression;
                    break;
                default:
                    throw new ArgumentException("Not a member access", "member");
            }
            return memberExpr;
        }
    }
}