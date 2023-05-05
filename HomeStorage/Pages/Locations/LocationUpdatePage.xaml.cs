using HomeStorage.InternalAPI;
using HomeStorage.ModelExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace HomeStorage.Pages.Locations;

[QueryProperty(nameof(LocationIdString), "Id")]
public partial class LocationUpdatePage : ContentPage
{
    public string LocationIdString
    {
        set => LoadLocationByIdAsync(value);
    }

    private LocationModel _location;
    public LocationUpdatePage()
    {
        InitializeComponent();
    }

    public async void LoadLocationByIdAsync(string locationIdString)
    {
        Guid.TryParse(locationIdString, out Guid locationId);
        _location = await HSAPI.Location.GetLocationByIdAsync(locationId);

        //Initialize either edit or new
        LocationImagePreview.Source = ImageSource.FromUri(_location.ImageUrl);
        LocationName.Text = _location.Name;
    }

    private async void PickImageAsync(object sender, EventArgs e)
    {
        FileResult file = await MediaPicker.PickPhotoAsync();

        if (file == null)
            return;

        Stream stream = await file.OpenReadAsync();
        LocationImagePreview.Source = ImageSource.FromStream(() => stream);

        using MemoryStream memoryStream = new();
        await stream.CopyToAsync(memoryStream);
        _location.NewImage = memoryStream.ToArray();

    }

    private async void CreateNewLocationAsync(object sender, EventArgs e)
    {
        _location.Name = LocationName.Text;

        ResponseModel response = await HSAPI.Location.CreateNewLocationAsync(_location);

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

    private async void BackToLocationListAsync(object sender, EventArgs e) => await Navigation.PopAsync();
}