using System.ComponentModel;

namespace HomeStorage.Logic.Models.ProductModels
{
    public class ProductFilterModel
    {
        public string? OrderByProperty { get; set; }
        public ListSortDirection SortDirection { get; set; } = ListSortDirection.Ascending;
        public List<Guid> Categories { get; set; } = [];
        public double? MaxAmount { get; set; }
        public double? MinAmount { get; set; }
        public string? SearchString { get; set; }
    }
}
