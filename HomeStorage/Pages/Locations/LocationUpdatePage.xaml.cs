using HomeStorage.InternalAPI;
using HomeStorage.ModelExtensions;
using HomeStorage.ViewModels.Locations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace HomeStorage.Pages.Locations;

[QueryProperty(nameof(LocationIdString), "Id")]
public partial class LocationUpdatePage : ContentPage
{
    private LocationViewModel _viewModel = new();
    public string LocationIdString { get; set; }

    public LocationUpdatePage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Guid.TryParse(LocationIdString, out Guid locationId);
        await _viewModel.LoadLocationByIdAsync(locationId);
    }

    private async void PickImageAsync(object sender, EventArgs e)
    {
        FileResult file = await MediaPicker.PickPhotoAsync();

        if (file == null)
            return;

        Stream stream = await file.OpenReadAsync();
        using MemoryStream memoryStream = new();
        await stream.CopyToAsync(memoryStream);
        _viewModel.Location.NewImage = memoryStream.ToArray();
    }

    private async void CreateNewLocationAsync(object sender, EventArgs e)
    {
        ResponseModel response = await HSAPI.Location.CreateNewLocationAsync(_viewModel.Location);

        if (response.Success)
        {
            await DisplayAlert("Lokation oprettet", response.Message, "Ok");
            Navigation.RemovePage(this);
            await Navigation.PushAsync(new LocationListPage());
        }
        else
        {
            await DisplayAlert("Fejl", response.Message, "Ok");
        }
    }

    private async void BackToLocationListAsync(object sender, EventArgs e) => 
        await Shell.Current.GoToAsync(HSAPI.Routing.Pages[typeof(LocationListPage)]);
}