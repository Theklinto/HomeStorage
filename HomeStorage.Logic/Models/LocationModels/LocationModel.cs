using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Interfaces;

namespace HomeStorage.Logic.Models.LocationModels
{
    public class LocationModel : IDTO<LocationModel, Location>
    {
        public Guid? LocationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? ImageId { get; set; }

        public static LocationModel AsDTO(Location source)
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
