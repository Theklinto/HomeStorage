using HomeStorage.Logic.Services;
using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Models.CategoryModels;
using HomeStorage.Logic.Interfaces;

namespace HomeStorage.Logic.Models.ProductModels
{
    public class ProductModel : IDTO<ProductModel, Product>
    {
        public Guid? ProductId { get; set; }
        public Guid? LocationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? ImageId { get; set; }
        public List<CategoryNotationModel> Categories { get; set; } = new();
        public DateTime? ExpirationDate { get; set; }
        public double Amount { get; set; }


        public static ProductModel AsDTO(Product source)
        {
            return new()
            {
                Amount = source.Amount,
                Categories = source.Categories
                    .Select(DTOService.AsDTO<CategoryNotationModel, Category>)
                    .ToList(),
                Description = source.Description,
                ExpirationDate = source.ExpirationDate,
                ImageId = source.ImageId,
                LocationId = source.LocationId,
                Name = source.Name,
                ProductId = source.ProductId,
            };
        }
    }
}
