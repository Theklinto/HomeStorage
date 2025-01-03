﻿using HomeStorage.DataAccess.LocationEntities;
using HomeStorage.DataAccess.ProductEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStorage.DataAccess.CategoryEntities
{
    public class Category
    {
        [Key]
        public Guid CategoryId { get; set; }
        [Required, MinLength(3)]
        public required string Name { get; set; }
        [Required]
        public required Guid LocationId { get; set; }


        [Required, ForeignKey(nameof(LocationId))]
        public virtual Location Location { get; set; } = default!;
        public virtual List<Product> Products { get; set; } = [];
    }
}
