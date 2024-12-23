namespace HomeStorage.Logic.Models.LocationModels
{
    public class LocationUserModel
    {
        public Guid LocationUserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid LocationId { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsOwner { get; set; } = false;
    }
}
