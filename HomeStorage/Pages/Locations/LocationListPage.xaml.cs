using HomeStorage.InternalAPI;
using HomeStorage.ModelExtensions;
using HomeStorage.ViewModels.Locations;
using System.Collections.ObjectModel;

namespace HomeStorage.Pages.Locations;

public partial class LocationListPage : ContentPage
{
    private LocationListViewModel _viewModel = new();
    public LocationListPage()
    {
        BindingContext = _viewModel;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadLocations();
    }

    private async void NewLocationAsync(object sender, EventArgs e) => 
        await Shell.Current.GoToAsync($"/{HSAPI.Routing.Pages[typeof(LocationUpdatePage)]}?Id={Guid.Empty}");
}