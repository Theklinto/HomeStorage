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

        await DeviceStorage.SetAsync(EStorageKey.Email, EmailEntryField.Text);
        await DeviceStorage.SetAsync(EStorageKey.Password, PasswordEntryField.Text);
        await DeviceStorage.SetAsync(EStorageKey.JwtToken, jwtTokenModel.Token);
        await DeviceStorage.SetAsync(EStorageKey.JwtTokenExpiration, jwtTokenModel.Expiration.ToString());

        ActivityIndicator.IsRunning = false;
        ActivityIndicatorPanel.IsVisible = false;

        Application.Current.MainPage = new AppShell();
    }

    private async void RegisterClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        return;
    }
}