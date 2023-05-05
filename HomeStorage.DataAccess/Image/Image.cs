using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.Entities
{
    public partial class Image
    {
        public Guid ImageId { get; set; }
        public string Path { get; set; } = string.Empty;
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = string.Empty;
        public virtual IdentityUser User { get; set; } = default!;
    }
}
