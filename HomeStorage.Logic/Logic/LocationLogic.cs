using HomeStorage.DataAccess.LocationEntities;
using HomeStorage.DataAccess.UserEntities;
using HomeStorage.Logic.Abstracts;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Enums;
using HomeStorage.Logic.Exceptions;
using HomeStorage.Logic.Models.LocationModels;
using HomeStorage.Logic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Logic.Logic
{
    public class LocationLogic(
        HomeStorageDbContext db,
        ImageLogic imageLogic,
        UserManager<HomeStorageUser> userManager,
        HttpContextService httpContextService) : LogicBase(httpContextService, db)
    {
        private readonly ImageLogic _imageLogic = imageLogic;
        private readonly UserManager<HomeStorageUser> _userManager = userManager;

        public async Task<LocationModel> CreateLocationAsync(LocationUpdateModel model)
        {
            if (model.LocationId.HasValue)
                throw new Exception("Can't create a location with predefined locationId");
            if (string.IsNullOrWhiteSpace(model.Name))
                throw new Exception("Can't create a location without a name");

            HomeStorageUser user = await GetCurrentUser();

            //Check for image and create
            Guid? imageId = default;
            if (model.NewImage is not null && model.NewImage.Length > 0)
                imageId = await _imageLogic.CreateImage(model.NewImage);

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
                UserId = user.Id,
                LocationId = location.LocationId,

            };
            await _db.LocationUsers.AddAsync(locationUser);
            await _db.SaveChangesAsync();

            await _db.Entry(location).ReloadAsync();

            return DTOService.AsDTO<LocationModel, Location>(location);
        }

        public async Task<LocationModel?> GetLocationAsync(Guid locationId)
        {
            HomeStorageUser user = await GetCurrentUser();

            LocationUser? locationUser = await _db.LocationUsers
                .Include(x => x.Location.Image)
                .FirstOrDefaultAsync(x => x.LocationId == locationId && x.UserId == user.Id);

            return locationUser is not null ?
                DTOService.AsDTO<LocationModel, Location>(locationUser.Location) : null;
        }

        public async Task<List<LocationModel>> GetAllLocationsByUser(HomeStorageUser? user = default)
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
            HomeStorageUser user = await GetCurrentUser();

            return await _db.LocationUsers
                .Where(x => x.UserId == user.Id)
                .Select(x => x.Location)
                .Select(x => new LocationListModel()
                {
                    Description = x.Description,
                    ImageId = x.ImageId,
                    IsAdmin = x.LocationUsers.First(x => x.UserId == user.Id).IsLoactionAdmin,
                    IsOwner = x.LocationUsers.First(x => x.UserId == user.Id).IsLocationOwner,
                    LocationId = x.LocationId,
                    Name = x.Name
                }).ToListAsync();
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
                location.ImageId = await _imageLogic.CreateImage(updateModel.NewImage);

            location.Name = updateModel.Name;
            location.Description = updateModel.Description;

            await _db.SaveChangesAsync();

            return DTOService.AsDTO<LocationModel, Location>(location);
        }

        public async Task<(LocationModel? model, bool? allowDeletion)> DeleteLocation(Guid locationId)
        {
            HomeStorageUser user = await GetCurrentUser();

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

        public async Task<LocationUserManagmentModel> GetLocationUsersAsync(Guid locationId)
        {
            if (await CheckUserAccess<Location>(locationId, EAccess.LocationAdmin) is false)
                throw new NotAuthenticatedException("You don't have the required access level");

            HomeStorageUser user = await GetCurrentUser();

            Dictionary<Guid, LocationUserListModel> users = _db.LocationUsers
                .Where(x => x.LocationId == locationId)
                .Include(x => x.User)
                .ToDictionary(x => x.UserId, x => new LocationUserListModel()
                {
                    Email = x.User.Email,
                    IsAdmin = x.IsLoactionAdmin,
                    IsOwner = x.IsLocationOwner,
                    LocationUserId = x.LocationUserId,
                    Username = x.User.UserName
                });

            LocationUserManagmentModel model = new()
            {
                Users = [.. users.Select(x => x.Value)],
                LocationAdmin = users[user.Id].IsAdmin,
                LocationOwner = users[user.Id].IsOwner,
            };

            return model;
        }

        public async Task<LocationUserModel?> AddUserToLocation(Guid locationId, string email)
        {
            if (await CheckUserAccess<Location>(locationId, EAccess.LocationAdmin) is false)
                return null;


            //Find user by email
            HomeStorageUser? addedUser = await _userManager.FindByEmailAsync(email);
            if (addedUser is null)
                return null;

            Location? location = await _db.Locations
                .Include(x => x.LocationUsers.Where(x => x.UserId == addedUser.Id))
                .FirstOrDefaultAsync(x => x.LocationId == locationId);

            if (location is null || location.LocationUsers.Count > 0)
                return null;

            LocationUser locationUser = new()
            {
                IsLoactionAdmin = true,
                LocationId = locationId,
                UserId = addedUser.Id,
            };
            location.LocationUsers.Add(locationUser);

            await _db.SaveChangesAsync();

            LocationUserModel model = new()
            {
                Email = addedUser.Email,
                IsAdmin = locationUser.IsLoactionAdmin,
                IsOwner = locationUser.IsLocationOwner,
                LocationId = locationId,
                LocationUserId = locationUser.LocationUserId,
                UserId = addedUser.Id,
                Username = addedUser.UserName
            };

            return model;
        }

        public async Task<LocationUserModel?> DeleteUserFromLocation(Guid locationUserId)
        {
            LocationUser? locationUser = await _db.LocationUsers
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.LocationUserId == locationUserId);

            bool hasAccess = await CheckUserAccess<Location>(locationUser?.LocationId, EAccess.LocationAdmin);

            if (locationUser is null || hasAccess is false)
                return null;

            _db.Remove(locationUser);
            await _db.SaveChangesAsync();

            LocationUserModel model = new()
            {
                Email = locationUser.User.Email,
                IsAdmin = locationUser.IsLoactionAdmin,
                IsOwner = locationUser.IsLocationOwner,
                LocationId = locationUserId,
                LocationUserId = locationUserId,
                UserId = locationUserId,
                Username = locationUser.User.UserName
            };

            return model;
        }
    }
}
