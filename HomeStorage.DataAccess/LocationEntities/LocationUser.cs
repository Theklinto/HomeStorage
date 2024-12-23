using HomeStorage.DataAccess.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStorage.DataAccess.LocationEntities
{
    public partial class LocationUser
    {
        [Key]
        public Guid LocationUserId { get; set; }
        public bool IsLoactionAdmin { get; set; }
        public bool IsLocationOwner { get; set; }
        [Required]
        public required Guid UserId { get; set; }
        [Required]
        public required Guid LocationId { get; set; }



        [Required, ForeignKey(nameof(UserId))]
        public virtual HomeStorageUser User { get; set; } = default!;
        [Required, ForeignKey(nameof(LocationId))]
        public virtual Location Location { get; set; } = default!;
    }
}
