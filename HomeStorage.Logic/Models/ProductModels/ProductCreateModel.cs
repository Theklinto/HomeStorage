using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HomeStorage.Logic.Models.ProductModels
{
    public class ProductCreateModel
    {
        [Required]
        public Guid LocationId { get; set; }
        [Required, MinLength(3)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<LookupModel<Guid?>> Categories { get; set; } = [];
        public DateTime? ExpirationDate { get; set; }
        public double? Amount { get; set; }
        [JsonIgnore]
        public IFormFile? Image { get; set; }
    }
}
