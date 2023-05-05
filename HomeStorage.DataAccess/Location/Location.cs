using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.Entities
{
    public partial class Location
    {
        public string Name { get; set; } = string.Empty;
        public Guid LocationId { get; set; }
        [ForeignKey(nameof(Image))]
        public Guid? ImageId { get; set; }
        public virtual Image? Image { get; set; }
        public virtual List<LocationUser> LocationUsers { get; set; } = new List<LocationUser>();

    }
}
