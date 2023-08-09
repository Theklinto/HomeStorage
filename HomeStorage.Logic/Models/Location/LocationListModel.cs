using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HomeStorage.Logic.Models.Location
{
    public class LocationListModel
    {
        public Guid LocationId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? ImageId { get; set; }
        public bool AllowUserManagment { get; set; }
    }
}
