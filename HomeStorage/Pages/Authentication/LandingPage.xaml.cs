using HomeStorage.DataAccess.AuthenticationModels;
using HomeStorage.InternalAPI;
using HomeStorage.Pages.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using DeviceStorage = HomeStorage.SecureStorageExtension;

namespace HomeStorage.Pages.Authentication;

public partial class LandingPage : ContentPage
{
    public LandingPage()
    {
        InitializeComponent();
        CheckLoginStatus();
    }

    public static async void CheckLoginStatus()
    {
        static async Task GoToLogin() => await Shell.Current.GoToAsync(HSAPI.Routing.UnauthenticatedPages[typeof(SignInPage)]);

        string email = DeviceStorage.GetString(EStorageKey.Email) ?? string.Empty;
        string password = DeviceStorage.GetString(EStorageKey.Password) ?? string.Empty;
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            await GoToLogin();
            return;
        }

        DateTime? jwtTokenExpiration = DeviceStorage.GetDate(EStorageKey.JwtTokenExpiration);
        if (jwtTokenExpiration.GetValueOrDefault() < DateTime.Now)
        {
            JwtTokenModel response = await HSAPI.Authentication.Login(
                new()
                {
                    Email = email,
                    Password = password
                });

            if (string.IsNullOrWhiteSpace(response.Token))
            {
                HSAPI.Authentication.PurgeAuthenticationStorage();
                await GoToLogin();
                return;
            }

            DeviceStorage.Set(EStorageKey.JwtToken, response.Token);
            DeviceStorage.Set(EStorageKey.JwtTokenExpiration, response.Expiration);
        }

        Application.Current.MainPage = new AuthenticatedAppShell();
    }
}