using HomeStorage.DataAccess.AuthenticationModels;
using HomeStorage.InternalAPI;
using DeviceStorage = HomeStorage.SecureStorageExtension;

namespace HomeStorage.Pages.Authentication;

public partial class SignInPage : ContentPage
{
	public SignInPage()
	{
		InitializeComponent();
	}

    private async void SignInClicked(object sender, EventArgs e)
    {
        ActivityIndicator.IsRunning = true;
        ActivityIndicatorPanel.IsVisible = true;

        JwtTokenModel jwtTokenModel = await HSAPI.Authentication.Login(new()
        {
            Email = EmailEntryField.Text,
            Password = PasswordEntryField.Text
        });

        if(jwtTokenModel.Token is null)
        {
            ActivityIndicatorPanel.IsVisible = false;
            ActivityIndicator.IsRunning = false;
            await DisplayAlert("Forkerte login oplysninger!", "Vi kunne ikke logge dig ind, med de indtastede oplysninger", "Ok");
            return;
        }

        DeviceStorage.Set(EStorageKey.Email, EmailEntryField.Text);
        DeviceStorage.Set(EStorageKey.Password, PasswordEntryField.Text);
        DeviceStorage.Set(EStorageKey.JwtToken, jwtTokenModel.Token);
        DeviceStorage.Set(EStorageKey.JwtTokenExpiration, jwtTokenModel.Expiration);

        ActivityIndicator.IsRunning = false;
        ActivityIndicatorPanel.IsVisible = false;

        Application.Current.MainPage = new AuthenticatedAppShell();
    }

    private async void RegisterClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        return;
    }
}