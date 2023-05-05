using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.Entities
{
    public class LocationUserModel
    {
        public Guid? LocationUserId { get; set; }
        public bool? IsLoactionAdmin { get; set; }
        public bool? IsLocationOwner { get; set; }
        public Guid? UserId { get; set; }
        public Guid? LocationId { get; set; }
    }
}
