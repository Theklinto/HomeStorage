namespace HomeStorage.API.Models
{
    public class VersionModel
    {
        public VersionModel(string? version)
        {
            Version = version ?? string.Empty;
        }
        public string Version { get; set; }
    }
}
