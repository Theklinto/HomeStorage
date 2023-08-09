using HomeStorage.InternalAPI;
using HomeStorage.ModelExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.ViewModels.Locations
{
    public class LocationViewModel : ViewModelBase
    {
        private LocationModel _location = new();
        public LocationModel Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadLocationByIdAsync(Guid locationId)
        {
            Location = await HSAPI.Location.GetLocationByIdAsync(locationId);
        }
    }
}
