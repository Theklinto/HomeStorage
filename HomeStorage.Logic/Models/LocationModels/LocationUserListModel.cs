namespace HomeStorage.Logic.Models.LocationModels
{
    public class LocationUserListModel
    {
        public Guid LocationUserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsOwner { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
    }
}
