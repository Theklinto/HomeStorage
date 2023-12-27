using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Abstracts;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Enums;
using HomeStorage.Logic.Models.LocationModels;
using HomeStorage.Logic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Logic.Logic
{
    public class LocationLogic : LogicBase
    {
        private readonly ImageLogic _imageLogic;
        private readonly UserManager<IdentityUser> _userManager;
        public LocationLogic(HomeStorageDbContext dbContext, ImageLogic imageLogic, UserManager<IdentityUser> userManager,
            HttpContextService httpContextService) : base(httpContextService, dbContext)
        {
            _imageLogic = imageLogic;
            _userManager = userManager;
        }

        public async Task<LocationModel> CreateLocationAsync(LocationUpdateModel model)
        {
            if (model.LocationId.HasValue)
                throw new Exception("Can't create a location with predefined locationId");
            if (string.IsNullOrWhiteSpace(model.Name))
                throw new Exception("Can't create a location without a name");

            IdentityUser user = await GetCurrentUser();

            //Check for image and create
            Guid? imageId = default;
            if (model.NewImage is not null && model.NewImage.Length > 0)
                imageId = await _imageLogic.CreateImageAsync(model.NewImage);

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

            return DTOService.AsDTO<LocationModel, Location>(location);
        }

        public async Task<LocationModel?> GetLocationAsync(Guid locationId)
        {
            IdentityUser user = await GetCurrentUser();

            LocationUser? locationUser = await _db.LocationUsers
                .Include(x => x.Location.Image)
                .FirstOrDefaultAsync(x => x.LocationId == locationId && x.UserId == user.Id);

            return locationUser is not null ?
                DTOService.AsDTO<LocationModel, Location>(locationUser.Location) : null;
        }

        public async Task<List<LocationModel>> GetAllLocationsByUser(IdentityUser? user = default)
        {
            user ??= await GetCurrentUser();

            List<Location> locations = await _db.LocationUsers
                .Where(x => x.UserId == user.Id)
                .Include(x => x.Location.Image)
                .Select(x => x.Location)
                .ToListAsync();

            return locations
                .Select(DTOService.AsDTO<LocationModel, Location>)
                .ToList();
        }

        public async Task<List<LocationListModel>> GetList()
        {
            IdentityUser user = await GetCurrentUser();
            List<Location> locations = await _db.Locations
                .Where(x => x.LocationUsers.Any(y => y.UserId == user.Id))
                .Include(x => x.LocationUsers)
                .Include(x => x.Image)
                .ToListAsync();

            List<LocationListModel> result = locations
                .Select(DTOService.AsDTO<LocationListModel, Location>)
                .ToList();

            result.ForEach(location =>
            {
                location.AllowUserManagment = locations
                    .Where(x => x.LocationId == location.LocationId)
                    .Where(x => x.LocationUsers.Any(x => x.UserId == user.Id && (x.IsLocationOwner || x.IsLocationOwner)))
                    .Any();
            });

            return result;
        }

        public async Task<LocationModel?> UpdateLocation(LocationUpdateModel updateModel)
        {
            Location? location = await _db.Locations
                .Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.LocationId == updateModel.LocationId);

            if (location is null)
                return null;

            if (location.Image is not null && updateModel.NewImage is not null)
                await _imageLogic.UpdateImageAsync(location.Image.ImageId, updateModel.NewImage);
            else if (location.Image is null && updateModel.NewImage is not null)
                location.ImageId = await _imageLogic.CreateImageAsync(updateModel.NewImage);

            location.Name = updateModel.Name;
            location.Description = updateModel.Description;

            await _db.SaveChangesAsync();

            return DTOService.AsDTO<LocationModel, Location>(location);
        }

        public async Task<(LocationModel? model, bool? allowDeletion)> DeleteLocation(Guid locationId)
        {
            IdentityUser user = await GetCurrentUser();

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
            return (DTOService.AsDTO<LocationModel, Location>(location), true);
        }

        public async Task<List<LocationUserModel>?> GetLocationUsersAsync(Guid locationId)
        {
            if (await CheckUserAccess<Location>(locationId, EAccess.LocationAdmin) is false)
                return null;

            Location? location = await _db.Locations
                .FirstOrDefaultAsync(x => x.LocationId == locationId);

            if (location is null)
                return null;

            return location.LocationUsers
                .Select(DTOService.AsDTO<LocationUserModel, LocationUser>)
                .ToList();
        }

        public async Task<LocationUserModel?> AddUserToLocation(LocationUserModel model)
        {
            if (await CheckUserAccess<Location>(model.LocationId, EAccess.LocationAdmin) is false)
                return null;

            Location? location = await _db.Locations
                .FirstOrDefaultAsync(x => x.LocationId == model.LocationId);

            //Find user by email
            IdentityUser? addedUser = await _userManager.FindByEmailAsync(model.Email ?? string.Empty);

            if (location is null || addedUser is null)
                return null;

            LocationUser locationUser = new()
            {
                IsLoactionAdmin = true,
                LocationId = model.LocationId,
                UserId = addedUser.Id,
            };
            location.LocationUsers.Add(locationUser);

            await _db.SaveChangesAsync();

            return DTOService.AsDTO<LocationUserModel, LocationUser>(locationUser);
        }

        public async Task<LocationUserModel?> DeleteUserFromLocation(Guid locationUserId)
        {
            LocationUser? locationUser = await _db.LocationUsers
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.LocationUserId == locationUserId);

            bool hasAccess = await CheckUserAccess<Location>(locationUser?.LocationId, EAccess.LocationAdmin);

            if (locationUser is null || hasAccess is false)
                return null;

            _db.Remove(locationUser);
            await _db.SaveChangesAsync();

            return DTOService.AsDTO<LocationUserModel, LocationUser>(locationUser);
        }
    }
}
