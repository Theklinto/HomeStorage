﻿using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Interfaces;

namespace HomeStorage.Logic.Models.LocationModels
{
    public class LocationUserModel : IDTO<LocationUserModel, LocationUser>
    {
        public Guid LocationUserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid LocationId { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsOwner { get; set; } = false;

        public static LocationUserModel AsDTO(LocationUser source)
        {
            return new()
            {
                Email = source.User?.Email ?? string.Empty,
                LocationId = source.LocationId,
                LocationUserId = source.LocationUserId,
                UserId = Guid.Parse(source.UserId),
                Username = source.User?.UserName ?? string.Empty,
                IsAdmin = source.IsLoactionAdmin,
                IsOwner = source.IsLocationOwner
            };
        }
    }
}
