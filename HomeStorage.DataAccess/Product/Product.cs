using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.Entities
{
    public partial class Product
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
