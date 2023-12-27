using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Models.ProductModels
{
    public class ProductUpdateModel : ProductModel
    {
        [JsonIgnore]
        public IFormFile? NewImage { get; set; }
    }
}
