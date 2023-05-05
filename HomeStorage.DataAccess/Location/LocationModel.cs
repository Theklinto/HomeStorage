using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.Entities
{
    public class LocationModel
    {
        public Guid LocationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? ImageId { get; set; }
        public byte[]? NewImage { get; set; }
        public List<LocationUserModel> LocationUsers { get; set; } = new List<LocationUserModel>();
    }
}
