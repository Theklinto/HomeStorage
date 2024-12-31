namespace HomeStorage.Logic.Models.CategoryModels
{
    public class CategoryModel
    {
        public required Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public required Guid LocationId { get; set; }
    }
}
