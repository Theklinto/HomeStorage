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
using HomeStorage.Logic.Abstracts;
using HomeStorage.Logic.Services;

namespace HomeStorage.Logic.Logic
{
    public class ImageLogic : LogicBase
    {
        public ImageLogic(HomeStorageDbContext dbContext, HttpContextService httpContextService)
            : base(httpContextService, dbContext) { }

        public async Task<Guid> CreateImageAsync(IFormFile file)
        {
            using MemoryStream memoryStream = new();
            await file.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            if (imageBytes.Length <= 0)
                return new();

            Image? image = new()
            {
                User = await GetCurrentUser(),
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
