namespace HomeStorage.Logic.Models.ProductModels
{
    public class ProductListModel
    {
        public required Guid ProductId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public double? Amount { get; set; }
    }
}
