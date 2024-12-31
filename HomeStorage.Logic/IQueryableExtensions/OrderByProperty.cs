using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace HomeStorage.Logic.IQueryableExtensions
{
    public static class IQueryableExtensions
    {
        public static IOrderedQueryable<T> OrderByProperty<T>(this IQueryable<T> query, string? propertyName, ListSortDirection sortDirection, Expression<Func<T, object?>> fallback)
        {
            PropertyInfo[] typeProperties = typeof(T).GetProperties();
            PropertyInfo? property = typeProperties
                .FirstOrDefault(x => x.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));


            Expression<Func<T, object?>> orderByFn = fallback;
            if (property is not null)
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
                MemberExpression propertyAccessor = Expression.Property(parameter, property);
                Expression convertedProperty = Expression.Convert(propertyAccessor, typeof(object));

                orderByFn = Expression.Lambda<Func<T, object?>>(convertedProperty, parameter);
            }

            if (sortDirection == ListSortDirection.Descending)
                return query.OrderByDescending(orderByFn);

            return query.OrderBy(orderByFn);

        }
    }
}
