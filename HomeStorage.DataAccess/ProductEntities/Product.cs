using HomeStorage.DataAccess.CategoryEntities;
using HomeStorage.DataAccess.ImageEntities;
using HomeStorage.DataAccess.LocationEntities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace HomeStorage.DataAccess.ProductEntities
{
    public partial class Product
    {
        public required Guid ProductId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public Guid? ImageId { get; set; }
        public required Guid LocationId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public double Amount { get; set; }


        [Required, ForeignKey(nameof(LocationId))]
        public virtual Location Location { get; set; } = default!;
        public virtual List<Category> Categories { get; set; } = [];
        [ForeignKey(nameof(ImageId))]
        public virtual Image? Image { get; set; }



        //EF Extensions
        public static Expression<Func<Product, bool>> ContainsSearchString(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return x => true;
            string searchExpression = $"%{searchString}%";

            return x =>
                EF.Functions.Like(x.Name, searchExpression) ||
                EF.Functions.Like(x.Description, searchExpression);
        }
    }

}
