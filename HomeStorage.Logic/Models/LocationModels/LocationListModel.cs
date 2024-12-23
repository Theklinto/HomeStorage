namespace HomeStorage.Logic.Models.LocationModels
{
    public class LocationListModel
    {
        public Guid LocationId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? ImageId { get; set; }
        public bool IsOwner { get; set; }
        public bool IsAdmin { get; set; }
    }
}
