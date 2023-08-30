using IQueryableFilter.Attributes;
using IQueryableFilter.Enums;
using IQueryableFilter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IQueryableFilter.Extensions
{
    public static class IQueryableFilter
    {
        public static async Task<PaginationModel<T>> Extract<T>(this IQueryable<T> query, QueryFilter filter, CancellationToken cancellationToken = default) where T : class
        {
            int totalCount = await query.CountAsync(cancellationToken);
            List<T> result = await query
                .Skip(filter.Skip)
                .Take(filter.Take)
                .ToListAsync(cancellationToken);

            return new PaginationModel<T>
            {
                Data = result,
                TotalCount = totalCount
            };
        }
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, QueryFilter filter)
        {
            //Check if search string is empty
            if (string.IsNullOrWhiteSpace(filter.Search))
                return query;

            Type type = typeof(T);
            List<string> errors = new();

            //Set default expression to false for all
            Expression expressionBody = Expression.Constant(false);
            //Get entity that is queried on ([x] => x...)
            ParameterExpression entity = Expression.Parameter(type);

            //Loop through all properties that are specified in the filter
            //wrongly spelled property names will be ignored
            PropertyInfo[] properties = type.GetProperties()
                .Where(x => filter.PropertyNames.Contains(x.Name))
                .ToArray();
            foreach (PropertyInfo property in properties)
            {
                //Try get attribute that allows filtering
                QueryFilterableAttribute? propFilter = property
                    .GetCustomAttribute<QueryFilterableAttribute>();

                //Errors will be all be collected and returned as one, instead of the first hit
                #region Guard clauses
                // If property does not have the attribute decoration return error
                if (propFilter is null)
                {
                    errors.Add($"{type.Name}.{property.Name}: Does not contain a {nameof(QueryFilterableAttribute)}");
                    continue;
                }
                //If property doesnt allow the requested comparison type return error
                if (propFilter.AllowsComparison(filter.ComparisonType) is false)
                {
                    errors.Add($"{type.Name}.{property.Name}: Does not allow the ussage of ${Enum.GetName(filter.ComparisonType)} filter");
                    continue;
                }
                #endregion

                //Build out the expression based on the comparison type
                expressionBody = filter.ComparisonType switch
                {
                    EComparisonType.Like => Expression.OrElse(expressionBody, GetEFLikeExpression(entity, property, filter.SearchExpression)),
                    _ => expressionBody
                };
            }

            return query.Where(Expression.Lambda<Func<T, bool>>(expressionBody, entity));

        }

        private static Expression GetEFLikeExpression(ParameterExpression entity, PropertyInfo property, string searchExpression)
        {
            //Defined search expression argument and type
            ConstantExpression argument = Expression.Constant(searchExpression, typeof(string));
            //Define the property that is being used, and the entity it's attached to
            MemberExpression exProperty = Expression.Property(entity, property);
            //Find the method (EF.Functions.Like) so we can apply the arguments dynamicly
            MethodInfo efLikeMethod = typeof(DbFunctionsExtensions).GetMethod("Like",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(DbFunctions), typeof(string), typeof(string) },
                null)!;
            //Define the type and property the efLikeMethod should be called from (EF.Functions)
            MemberExpression efTypeWithMethod = Expression.Property(null, typeof(EF), nameof(EF.Functions));
            //Make the method call with the supplied arguments and placement
            Expression expr = Expression.Call(efLikeMethod, efTypeWithMethod, exProperty, argument);

            return expr;
        }
    }
}
