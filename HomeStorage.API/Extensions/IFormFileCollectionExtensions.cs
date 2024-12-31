namespace HomeStorage.API.Extensions
{
    public static class IFormFileCollectionExtensions
    {
        public static IFormFile? GetFirstFileOrDefault(this IFormFileCollection files)
        {
            return files.Count > 0 ? files[0] : null;
        }
    }
}
