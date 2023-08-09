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

        public async Task<Guid> CreateImageAsync(IFormFile file, IdentityUser user)
        {
            using MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            if (imageBytes.Length <= 0)
                return new();

            Image? image = new()
            {
                User = user,
                ImageBytes = imageBytes,
            };

            //Save and retrive id
            await _db.Images.AddAsync(image);
            await _db.SaveChangesAsync();

            return image.ImageId;
        }

        public async Task<Image> UpdateImageAsync(Guid imageId, IFormFile imageFile)
        {
            using MemoryStream memoryStream = new();
            await imageFile.CopyToAsync(memoryStream);

            Image image = await _db.Images.FirstOrDefaultAsync(x => x.ImageId == imageId) ?? throw new Exception("ImageId not found");
            image.ImageBytes = memoryStream.ToArray();

            await _db.SaveChangesAsync();

            return image;
        }

        public async Task<Image?> GetImageAsync(Guid ImageId)
        {
            Image? image = await _db.Images.FirstOrDefaultAsync(x => x.ImageId == ImageId);
            return image;
        }
    }
}
