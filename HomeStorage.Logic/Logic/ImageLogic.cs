using HomeStorage.DataAccess.ImageEntities;
using HomeStorage.DataAccess.UserEntities;
using HomeStorage.Logic.Abstracts;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HomeStorage.Logic.Logic
{
    public class ImageLogic(HomeStorageDbContext db, HttpContextService httpContextService) : LogicBase(httpContextService, db)
    {
        public async Task<Guid> CreateImageAsync(IFormFile file)
        {
            using MemoryStream memoryStream = new();
            await file.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();

            if (imageBytes.Length <= 0)
                return new();

            HomeStorageUser user = await GetCurrentUser();

            Image? image = new()
            {
                UserId = user.Id,
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
