using IQueryableFilter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQueryableFilter.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class QueryFilterableAttribute : Attribute
    {
        private readonly EComparisonType[] _comparisonTypes;
        public QueryFilterableAttribute(params EComparisonType[] allowComparisonTypes)
        {
            _comparisonTypes = allowComparisonTypes;
        }

        public bool AllowsComparison(EComparisonType comparisonType) => _comparisonTypes.Contains(comparisonType);
    }
}
