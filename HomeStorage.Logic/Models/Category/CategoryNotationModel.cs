using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Models.Category
{
    public class CategoryNotationModel
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
