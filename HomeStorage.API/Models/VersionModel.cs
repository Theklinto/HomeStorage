namespace HomeStorage.API.Models
{
    public class VersionModel(string? version)
    {
        public string Version { get; set; } = version ?? string.Empty;
    }
}
