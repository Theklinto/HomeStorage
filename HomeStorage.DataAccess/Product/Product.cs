using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [ForeignKey(nameof(Image))]
        public Guid? ImageId { get; set; }
        public Image? Image { get; set; }
        public virtual List<Category> Categories { get; set; } = new();
        [ForeignKey(nameof(Location))]
        public Guid LocationId { get; set; }
        public virtual Location Location { get; set; } = default!;
        public DateTime? ExpirationDate { get; set; }
        public double Amount { get; set; }
    }
}
