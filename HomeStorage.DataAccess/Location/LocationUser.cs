using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.Entities
{
    public partial class LocationUser
    {
        public Guid LocationUserId { get; set; }
        public bool IsLoactionAdmin { get; set; } = false;
        public bool IsLocationOwner { get; set; } = false;
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = string.Empty;
        public virtual IdentityUser User { get; set; } = default!;
        [ForeignKey(nameof(Location))]
        public Guid LocationId { get; set; }
        public virtual Location Location { get; set; } = default!;
    }
}
