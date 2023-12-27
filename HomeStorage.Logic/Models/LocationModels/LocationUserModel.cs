using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Models.LocationModels
{
    public class LocationUserModel : IDTO<LocationUserModel, LocationUser>
    {
        public Guid LocationUserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid LocationId { get; set; }

        public static LocationUserModel AsDTO(LocationUser source)
        {
            return new()
            {
                Email = source.User?.Email ?? string.Empty,
                LocationId = source.LocationId,
                LocationUserId = source.LocationUserId,
                UserId = Guid.Parse(source.UserId),
                Username = source.User?.NormalizedUserName ?? string.Empty,
            };
        }
    }
}
