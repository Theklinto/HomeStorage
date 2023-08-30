using IQueryableFilter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQueryableFilter.Models
{
    public class QueryFilter
    {
        public List<string> PropertyNames { get; set; } = new();
        public string Search = "";
        public string SearchExpression => ComparisonType == EComparisonType.Equal ? Search : $"%{Search}%";
        public EComparisonType ComparisonType { get; set; } = EComparisonType.Equal;
        public SortOrder SortOrder = new("", EOrder.Ascending);
    }
    public record SortOrder(string PropertyName, EOrder Order);
}
