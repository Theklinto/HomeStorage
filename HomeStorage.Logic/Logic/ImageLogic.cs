using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using HomeStorage.Logic.DbContext;
using Microsoft.EntityFrameworkCore;
using HomeStorage.DataAccess.Entities;

namespace HomeStorage.Logic.Logic
{
    public class ImageLogic
    {
        private readonly IConfiguration _config;
        private readonly HomeStorageDbContext _db;

        public ImageLogic(IConfiguration config, HomeStorageDbContext dbContext)
        {
            _config = config;
            _db = dbContext;
        }

        public async Task<Guid> CreateImageAsync(byte[] imageBytes, IdentityUser user)
        {
            if (imageBytes.Length <= 0)
                return new();

            string imageBasePath = _config.GetSection("ImagesPath").Value ?? string.Empty;
            if (string.IsNullOrWhiteSpace(imageBasePath))
                throw new Exception("No ImagePath is given");

            Image? image = new()
            {
                User = user,
            };

            //Save and retrive id
            await _db.Images.AddAsync(image);
            await _db.SaveChangesAsync();

            string path = Path.Combine(imageBasePath, image.ImageId.ToString() + ".png");
            using FileStream fileStream = new(path, FileMode.Create);
            await fileStream.WriteAsync(imageBytes);

            image.Path = path;
            await _db.SaveChangesAsync();

            return image.ImageId;
        }

        public async Task<string> GetImagePathAsync(Guid imageId)
        {
            Image? image = await _db.Images.FirstOrDefaultAsync(x => x.ImageId == imageId);
            return image is not null ?
                image.Path : string.Empty;
        }
    }
}
