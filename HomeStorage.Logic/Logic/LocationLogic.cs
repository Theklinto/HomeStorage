using AutoMapper;
using HomeStorage.DataAccess.AuthenticationModels;
using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Enums;
using HomeStorage.Logic.Models.Location;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Logic
{
    public class LocationLogic
    {
        private readonly HomeStorageDbContext _db;
        private readonly ImageLogic _imageLogic;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        public LocationLogic(HomeStorageDbContext dbContext, ImageLogic imageLogic, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _db = dbContext;
            _imageLogic = imageLogic;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<LocationModel> CreateLocationAsync(LocationUpdateModel model, IdentityUser user)
        {
            if (model.LocationId.HasValue)
                throw new Exception("Can't create a location with predefined locationId");
            if (string.IsNullOrWhiteSpace(model.Name))
                throw new Exception("Can't create a location without a name");

            //Check for image and create
            Guid? imageId = default;
            if (model.NewImage is not null && model.NewImage.Length > 0)
                imageId = await _imageLogic.CreateImageAsync(model.NewImage, user);

            Location location = new()
            {
                Name = model.Name,
                Description = model.Description,
                ImageId = imageId,
            };
            await _db.Locations.AddAsync(location);

            //Add creating user as owner
            LocationUser locationUser = new()
            {
                IsLocationOwner = true,
                IsLoactionAdmin = true,
                Location = location,
                User = user,
            };
            await _db.LocationUsers.AddAsync(locationUser);
            await _db.SaveChangesAsync();

            await _db.Entry(location).ReloadAsync();

            //Create model
            LocationModel result = _mapper.Map<LocationModel>(location);

            return result;
        }

        public async Task<LocationModel?> GetLocationAsync(Guid locationId, IdentityUser user)
        {
            LocationUser? locationUser = await _db.LocationUsers
                .Include(x => x.Location.Image)
                .FirstOrDefaultAsync(x => x.LocationId == locationId && x.UserId == user.Id);

            return locationUser is not null ?
                _mapper.Map<LocationModel>(locationUser.Location) : null;
        }

        public async Task<List<LocationModel>> GetAllLocationsByUser(IdentityUser user)
        {
            List<Location> locations = await _db.LocationUsers
                .Where(x => x.UserId == user.Id)
                .Include(x => x.Location.Image)
                .Select(x => x.Location)
                .ToListAsync();

            return _mapper.Map<List<LocationModel>>(locations);
        }

        public async Task<List<LocationListModel>> GetList(IdentityUser user)
        {
            List<Location> locations = await _db.Locations
                .Where(x => x.LocationUsers.Any(y => y.UserId == user.Id))
                .Include(x => x.LocationUsers)
                .Include(x => x.Image)
                .ToListAsync();

            List<LocationListModel> result = _mapper.Map<List<LocationListModel>>(locations);
            result.ForEach(location =>
            {
                location.AllowUserManagment = locations
                    .Where(x => x.LocationId == location.LocationId)
                    .Where(x => x.LocationUsers.Any(x => x.UserId == user.Id && (x.IsLocationOwner || x.IsLocationOwner)))
                    .Any();
            });

            return result;
        }

        public async Task<LocationModel?> UpdateLocation(LocationUpdateModel updateModel, IdentityUser user)
        {
            Location? location = await _db.Locations
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.LocationId == updateModel.LocationId);

            if (location is null)
                return null;

            if (location.Image is not null && updateModel.NewImage is not null)
                await _imageLogic.UpdateImageAsync(location.Image.ImageId, updateModel.NewImage);
            else if (location.Image is null && updateModel.NewImage is not null)
                location.ImageId = await _imageLogic.CreateImageAsync(updateModel.NewImage, user);

            location.Name = updateModel.Name;
            location.Description = updateModel.Description;

            await _db.SaveChangesAsync();

            return _mapper.Map<LocationModel>(location);
        }

        public async Task<(LocationModel? model, bool? allowDeletion)> DeleteLocation(Guid locationId, IdentityUser user)
        {
            Location? location = await _db.Locations
                .Include(x => x.Image)
                .Include(x => x.LocationUsers)
                .FirstOrDefaultAsync(x => x.LocationId == locationId);

            if (location is null)
                return (null, null);

            bool allowDeletion = location.LocationUsers
                .Any(x => x.UserId == user.Id && (x.IsLocationOwner || x.IsLoactionAdmin));

            if (allowDeletion is false)
                return (null, false);

            _db.LocationUsers.RemoveRange(location.LocationUsers);
            if (location.Image is not null)
                _db.Images.Remove(location.Image);
            _db.Locations.Remove(location);

            await _db.SaveChangesAsync();
            return (_mapper.Map<LocationModel>(location), true);
        }

        public async Task<bool> CheckUserAccess(Location? location, IdentityUser user, EAccess access)
        {
            //Load locationUsers if not preloaded
            if (location is null)
                return false;

            if (location.LocationUsers is null || location.LocationUsers.Any() is false)
                location.LocationUsers = await _db.LocationUsers
                    .Where(x => x.LocationId == location.LocationId)
                    .ToListAsync();

            Func<LocationUser, bool>? selector;

            switch (access)
            {
                case EAccess.LocationAdmin:
                case EAccess.Update:
                case EAccess.Delete:
                case EAccess.Create:
                    selector = (x) => x.UserId == user.Id && (x.IsLocationOwner || x.IsLoactionAdmin);
                    break;

                default:
                    selector = (x) => x.UserId == user.Id;
                    break;
            };

            bool hasAccess = location.LocationUsers.Any(selector);
            return hasAccess;
        }

        public async Task<List<LocationUserModel>?> GetLocationUsersAsync(Guid locationId, IdentityUser user)
        {
            Location? location = await _db.Locations
                .Include(x => x.LocationUsers)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.LocationId == locationId);

            bool hasAccess = await CheckUserAccess(location, user, EAccess.LocationAdmin);

            if (location is null || hasAccess is false)
                return null;

            return _mapper.Map<List<LocationUserModel>>(location.LocationUsers);
        }

        public async Task<LocationUserModel?> AddUserToLocation(LocationUserModel model, IdentityUser user)
        {
            Location? location = await _db.Locations
                .Include(x => x.LocationUsers)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.LocationId == model.LocationId);

            bool hasAccess = await CheckUserAccess(location, user, EAccess.LocationAdmin);

            //Find user by email
            IdentityUser? addedUser = await _userManager.FindByEmailAsync(model.Email ?? string.Empty);

            if (location is null || hasAccess is false || addedUser is null)
                return null;

            LocationUser locationUser = new()
            {
                IsLoactionAdmin = true,
                LocationId = model.LocationId,
                UserId = addedUser.Id,
            };
            location.LocationUsers.Add(locationUser);

            await _db.SaveChangesAsync();

            return _mapper.Map<LocationUserModel>(locationUser);
        }

        public async Task<LocationUserModel?> DeleteUserFromLocation(Guid locationUserId, IdentityUser user)
        {
            LocationUser? locationUser = await _db.LocationUsers
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.LocationUserId == locationUserId);

            bool hasAccess = await CheckUserAccess(locationUser?.Location, user, EAccess.LocationAdmin);

            if (locationUser is null || hasAccess is false)
                return null;

            _db.Remove(locationUser);
            await _db.SaveChangesAsync();

            return _mapper.Map<LocationUserModel?>(locationUser);
        }
    }
}
