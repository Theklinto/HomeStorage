using HomeStorage.DataAccess.CategoryEntities;
using HomeStorage.Logic.Interfaces;

namespace HomeStorage.Logic.Models.CategoryModels
{
    public class CategoryModel : IDTO<CategoryModel, Category>
    {
        public required Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public Guid? ImageId { get; set; }
        public required Guid LocationId { get; set; }

        public static CategoryModel AsDTO(Category source)
        {
            return new()
            {
                CategoryId = source.CategoryId,
                ImageId = source.ImageId,
                LocationId = source.LocationId,
                Name = source.Name,
            };
        }
    }
}
