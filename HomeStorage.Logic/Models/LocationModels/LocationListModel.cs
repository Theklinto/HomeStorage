using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Interfaces;
using Newtonsoft.Json;

namespace HomeStorage.Logic.Models.LocationModels
{
    public class LocationListModel : IDTO<LocationListModel, Location>
    {
        public Guid LocationId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? ImageId { get; set; }
        public bool AllowUserManagment { get; set; }

        public static LocationListModel AsDTO(Location source)
        {
            return new()
            {
                LocationId = source.LocationId,
                Name = source.Name,
                Description = source.Description,
                ImageId = source.ImageId,
            };
        }
    }
}
