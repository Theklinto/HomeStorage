using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Models.Category
{
    public class CategoryModel
    {
        public required Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public Guid? ImageId { get; set; }
        public required Guid LocationId { get; set; }
    }
}
