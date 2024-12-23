namespace HomeStorage.Logic.Models.LocationModels
{
    public class LocationUserManagmentModel
    {
        public bool LocationOwner { get; set; }
        public bool LocationAdmin { get; set; }
        public List<LocationUserListModel> Users { get; set; } = [];
    }
}
