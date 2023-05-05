using HomeStorage.InternalAPI;
using HomeStorage.ModelExtensions;
using HomeStorage.ViewModels.Locations;

namespace HomeStorage.Pages.Locations;

public partial class LocationListPage : ContentPage
{
    public LocationListPage()
    {
        InitializeComponent();
        RefreshLocationsAsync();
    }

    public async void RefreshLocationsAsync()
    {
        IEnumerable<LocationModel> locations = await HSAPI.Location.GetAllLocationsAsync();
        LocationListViewModel model = new()
        {
            Locations = locations
        };
        BindingContext = model;

        bool locationsInList = model.Locations.Any();
        //LocationListView.IsVisible = locationsInList;
        //EmptyListLabel.IsVisible = locationsInList is false;
    }

    private async void NewLocationAsync(object sender, EventArgs e) => await Shell.Current.GoToAsync($"/{HSAPI.Routing.Pages[typeof(LocationUpdatePage)]}?Id={Guid.Empty}");
}