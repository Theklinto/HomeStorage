using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Models.Category
{
    public class CategoryUpdateModel : CategoryModel
    {
        public IFormFile? NewImage { get; set; }
    }
}
