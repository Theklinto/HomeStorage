using HomeStorage.DataAccess.UserEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStorage.DataAccess.ImageEntities
{
    public partial class Image
    {
        [Key]
        public Guid ImageId { get; set; }
        [Required]
        public required Guid UserId { get; set; }
        [Required, MinLength(1)]
        public byte[] ImageBytes { get; set; } = [];


        [Required, ForeignKey(nameof(UserId))]
        public virtual HomeStorageUser User { get; set; } = default!;
    }
}
