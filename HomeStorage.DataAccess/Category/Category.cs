using HomeStorage.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        [ForeignKey(nameof(Image))]
        public Guid? ImageId { get; set; }
        public Image? Image { get; set; }
        [ForeignKey(nameof(Location))]
        public Guid LocationId { get; set; }
        public virtual Location Location { get; set; } = default!;
        public virtual List<Product> Products { get; set; } = new();
    }
}
