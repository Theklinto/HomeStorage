using HomeStorage.DataAccess.ImageEntities;
using HomeStorage.DataAccess.UserEntities;
using HomeStorage.Logic.Abstracts;
using HomeStorage.Logic.DbContext;
using HomeStorage.Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HomeStorage.Logic.Logic
{
    public class ImageLogic(HomeStorageDbContext db, HttpContextService httpContextService) : LogicBase(httpContextService, db)
    {
        public static string? GetImageUrl([NotNullIfNotNull(nameof(imageId))] Guid? imageId, DateTime? lastmodified)
        {
            if (imageId is null)
                return null;

            return $"/images/{imageId}?lastmodified={lastmodified:O}";
        }

        public async Task<Guid> CreateImage(IFormFile file)
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

        public Task<Image> CreateOrUpdateImage(IFormFile file, CancellationToken cancellation = default)
            => CreateOrUpdateImage(file, null, cancellation);
        public async Task<Image> CreateOrUpdateImage(IFormFile file, Guid? imageId = null, CancellationToken cancellationToken = default)
        {
            using MemoryStream memoryStream = new();
            await file.CopyToAsync(memoryStream, cancellationToken);
            byte[] imageBytes = memoryStream.ToArray();

            Image? image = imageId is null ? null : await _db.Images.FirstOrDefaultAsync(x => x.ImageId == imageId, cancellationToken);
            if (image is null && imageId is not null)
            {
                throw new Exception("Couldn't update image");
            }
            if (image is null)
            {
                HomeStorageUser user = await GetCurrentUser();
                image = new()
                {
                    UserId = user.Id,
                };
                await _db.Images.AddAsync(image, cancellationToken);
            }

            image.LastModified = DateTime.Now;
            image.ImageBytes = imageBytes;


            return image;
        }

        public async Task<Image> UpdateImageAsync(Guid imageId, IFormFile imageFile, CancellationToken cancellationToken = default)
        {
            using MemoryStream memoryStream = new();
            await imageFile.CopyToAsync(memoryStream, cancellationToken);

            Image image = await _db.Images.FirstOrDefaultAsync(x => x.ImageId == imageId, cancellationToken) ?? throw new Exception("ImageId not found");
            image.ImageBytes = memoryStream.ToArray();

            return image;
        }

        public async Task<Image?> GetImageAsync(Guid ImageId)
        {
            Image? image = await _db.Images.FirstOrDefaultAsync(x => x.ImageId == ImageId);
            return image;
        }
    }
}
