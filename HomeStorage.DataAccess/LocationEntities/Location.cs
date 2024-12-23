using HomeStorage.DataAccess.CategoryEntities;
using HomeStorage.DataAccess.ImageEntities;
using HomeStorage.DataAccess.ProductEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStorage.DataAccess.LocationEntities
{
    public partial class Location
    {
        [Key]
        public Guid LocationId { get; set; }
        [Required, MinLength(3)]
        public required string Name { get; set; }
        public Guid? ImageId { get; set; }
        public string? Description { get; set; }


        [ForeignKey(nameof(ImageId))]
        public virtual Image? Image { get; set; }
        public virtual List<LocationUser> LocationUsers { get; set; } = [];
        public virtual List<Category> Categories { get; set; } = [];
        public virtual List<Product> Products { get; set; } = [];
    }
}
