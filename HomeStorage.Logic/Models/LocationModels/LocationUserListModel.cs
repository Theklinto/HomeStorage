using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Interfaces;

namespace HomeStorage.Logic.Models.LocationModels
{
    public class LocationUserListModel : IDTO<LocationUserListModel, LocationUser>
    {
        public Guid LocationUserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsOwner { get; set; } = false;
        public bool IsAdmin { get; set; } = false;

        public static LocationUserListModel AsDTO(LocationUser source)
        {
            return new()
            {
                Email = source.User.Email ?? string.Empty,
                IsAdmin = source.IsLoactionAdmin,
                IsOwner = source.IsLocationOwner,
                LocationUserId = source.LocationUserId,
                Username = source.User.UserName ?? string.Empty
            };
        }
    }
}
