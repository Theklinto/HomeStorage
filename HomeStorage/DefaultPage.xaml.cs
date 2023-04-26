using HomeStorage.DataAccess.AuthenticationModels;
using HomeStorage.InternalAPI;
using HomeStorage.Pages.Authentication;
using DeviceStorage = HomeStorage.SecureStorageExtension;

namespace HomeStorage;

public partial class DefaultPage : ContentPage
{
    public DefaultPage()
    {
        InitializeComponent();
        CheckLoginStatus();
    }

    public static async void CheckLoginStatus()
    {
        string email = await DeviceStorage.GetAsync(EStorageKey.Email);
        string password = await DeviceStorage.GetAsync(EStorageKey.Password);
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await Shell.Current.GoToAsync(HSAPI.Routing.Pages[typeof(SignInPage)]);
            return;
        }

        string jwtTokenExpirationString = await DeviceStorage.GetAsync(EStorageKey.JwtTokenExpiration);
        if (DateTime.TryParse(jwtTokenExpirationString, out DateTime jwtExpiration) && jwtExpiration > DateTime.Now)
        {
            JwtTokenModel response = await HSAPI.Authentication.Login(
                new()
                {
                    Email = email,
                    Password = password
                });

            if (string.IsNullOrWhiteSpace(response.Token))
            {
                await Shell.Current.GoToAsync(HSAPI.Routing.Pages[typeof(SignInPage)]);
                return;
            }

            await DeviceStorage.SetAsync(EStorageKey.JwtToken, response.Token);
            await DeviceStorage.SetAsync(EStorageKey.JwtTokenExpiration, response.Expiration.ToString());
        }

        await Shell.Current.GoToAsync(HSAPI.Routing.Pages[typeof(MainPage)]);
    }
}