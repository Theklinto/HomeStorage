using HomeStorage.DataAccess.CategoryEntities;
using HomeStorage.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Models.CategoryModels
{
    public class CategoryNotationModel : IDTO<CategoryNotationModel, Category>
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public static CategoryNotationModel AsDTO(Category source)
        {
            return new()
            {
                CategoryId = source.CategoryId,
                Name = source.Name,
            };
        }
    }
}
