using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeStorage.InternalAPI;
using HomeStorage.ModelExtensions;

namespace HomeStorage.ViewModels.Locations
{
    public class LocationListViewModel
    {
        public IEnumerable<LocationModel> Locations { get; set; }
    }
}
