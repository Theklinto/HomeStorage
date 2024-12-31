namespace HomeStorage.Logic.Models.ProductModels
{
    public class ProductModel
    {
        public required Guid ProductId { get; set; }
        public required Guid LocationId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<LookupModel<Guid>> Categories { get; set; } = [];
        public DateTime? ExpirationDate { get; set; }
        public double? Amount { get; set; }
    }
}
