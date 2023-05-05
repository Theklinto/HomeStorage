using AutoMapper;
using HomeStorage.DataAccess.AuthenticationModels;
using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.DbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        public LocationLogic(HomeStorageDbContext dbContext, ImageLogic imageLogic, IMapper mapper)
        {
            _db = dbContext;
            _imageLogic = imageLogic;
            _mapper = mapper;
        }

        public async Task<LocationModel> CreateLocationAsync(LocationModel model, IdentityUser user)
        {
            if (model.LocationId != Guid.Empty)
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
            model = _mapper.Map<LocationModel>(location);

            return model;
        }

        public async Task<LocationModel?> GetLocationAsync(Guid locationId, IdentityUser user)
        {
            LocationUser? locationUser = await _db.LocationUsers
                .Include(x => x.Location)
                    .ThenInclude(x => x.LocationUsers)
                .FirstOrDefaultAsync(x => x.LocationId == locationId && x.UserId == user.Id);

            return locationUser is not null ?
                _mapper.Map<LocationModel>(locationUser.Location) : null;
        }

        public IEnumerable<LocationModel> GetAllLocationsByUser(IdentityUser user)
        {
            IEnumerable<Location> locations = _db.LocationUsers
                .Include(x => x.Location)
                .Where(x => x.UserId == user.Id)
                .Select(x => x.Location);

            return _mapper.Map<IEnumerable<LocationModel>>(locations);
        }
    }
}
