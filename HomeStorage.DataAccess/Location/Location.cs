using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.Entities
{
    public partial class Location
    {
        public required string Name { get; set; }
        public Guid LocationId { get; set; }
        [ForeignKey(nameof(Image))]
        public Guid? ImageId { get; set; }
        public virtual Image? Image { get; set; }
        public required string Description { get; set; }
        public virtual List<LocationUser> LocationUsers { get; set; } = new();
        public virtual List<Category> Categories { get; set; } = new();
        public virtual List<Product> Products { get; set; } = new();
    }
}
