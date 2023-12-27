using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Models.LocationModels
{
    public class LocationUpdateModel: LocationModel
    {
        public IFormFile? NewImage { get; set; }
    }
}
