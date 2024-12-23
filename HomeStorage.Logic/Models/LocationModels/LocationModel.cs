using HomeStorage.DataAccess.LocationEntities;
using HomeStorage.Logic.Interfaces;

namespace HomeStorage.Logic.Models.LocationModels
{
    public class LocationModel : IDTO<LocationModel, Location>
    {
        public Guid? LocationId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
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
