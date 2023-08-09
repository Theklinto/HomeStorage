using HomeStorage.Logic.Models.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Models.Product
{
    public class ProductModel
    {
        public Guid? ProductId { get; set; }
        public Guid? LocationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? ImageId { get; set; }
        public List<CategoryNotationModel> Categories { get; set; } = new();
        public DateTime? ExpirationDate { get; set; }
        public double Amount { get; set; }
    }
}
