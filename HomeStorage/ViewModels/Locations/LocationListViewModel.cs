using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeStorage.InternalAPI;
using HomeStorage.ModelExtensions;

namespace HomeStorage.ViewModels.Locations
{
    public class LocationListViewModel : ViewModelBase
    {
        private ObservableCollection<LocationModel> _locations = new();
        public ObservableCollection<LocationModel> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                OnPropertyChanged(nameof(Locations));
            }
        }

        public async Task LoadLocations()
        {
            Locations.Clear();
            IEnumerable<LocationModel> locations = await HSAPI.Location.GetAllLocationsAsync();
            Locations = new(locations);
        }
    }
}
