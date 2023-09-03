using IQueryableFilter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Filters
{
    public class HSSearchAsString : IGenericFilter
    {
        public string PropertyName { get; init; } = string.Empty;
        public IFilterComparer Comparer { get; init; } = HSFilterComparerCollection.StringContains;
        public string Search { get; init; } = string.Empty;
        public string FilterType { get; init; } = nameof(HSSearchAsString);

        private static readonly IList<Type> _allowedTypes = new[]
        {
            typeof(string),
            typeof(int),
        };

        private static readonly IList<IFilterComparer> _allowedFilterComparers = new[]
        {
            HSFilterComparerCollection.Equal,
            HSFilterComparerCollection.StringContains,
        };

        public bool AllowFilterComparer(IFilterComparer comparer)
        {
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            return _allowedFilterComparers.Any(x => x == comparer);
        }

        public bool AllowPropertyType(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return _allowedTypes.Contains(type);
        }

        public Expression? GetExpression(ParameterExpression parameter, PropertyInfo property)
        {
            if (Comparer == HSFilterComparerCollection.Equal)
                return GetEqualExpression(parameter, property, Search);
            else if (Comparer == HSFilterComparerCollection.StringContains)
                return GetContainsExpression(parameter, property, Search);

            return null;
        }

        private static Expression? GetContainsExpression(ParameterExpression parameter, PropertyInfo property, string searchExpression)
        {
            if (string.IsNullOrWhiteSpace(searchExpression))
                return null;

            //Defined search expression argument and type
            ConstantExpression argument = Expression.Constant(searchExpression, typeof(string));
            //Define the property that is being used, and the entity it's attached to
            MemberExpression exProperty = Expression.Property(parameter, property);

            Expression conversionExpression;
            //EF Can't translate ToString on strings, so we have to typecheck it
            if (property.PropertyType == typeof(string))
                conversionExpression = Expression.PropertyOrField(parameter, property.Name);
            else
                conversionExpression = Expression.Call(exProperty, nameof(ToString), null);

            Expression expr = Expression.Call(conversionExpression, nameof(string.Contains), null, argument);

            return expr;
        }

        private static Expression? GetEqualExpression(ParameterExpression parameter, PropertyInfo property, string searchExpression)
        {
            if (string.IsNullOrWhiteSpace(searchExpression))
                return null;

            //Defined search expression argument and type
            ConstantExpression argument = Expression.Constant(searchExpression, typeof(string));
            //Define the property that is being used, and the entity it's attached to
            MemberExpression exProperty = Expression.Property(parameter, property);

            Expression conversionExpression;
            //EF Can't translate ToString on strings, so we have to typecheck it
            if (property.PropertyType == typeof(string))
                conversionExpression = Expression.PropertyOrField(parameter, property.Name);
            else
                conversionExpression = Expression.Call(exProperty, nameof(ToString), null);

            return Expression.Equal(conversionExpression, argument);
        }
    }
}
