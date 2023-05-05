using DeviceStorage = HomeStorage.SecureStorageExtension;
namespace HomeStorage.Pages.Authentication;

public partial class SignOutPage : ContentPage
{
	public SignOutPage()
	{
		ClearLogin();
	}

	public static async void ClearLogin()
	{
		await DeviceStorage.SetAsync(EStorageKey.Username, "");
		await DeviceStorage.SetAsync(EStorageKey.Email, "");
		await DeviceStorage.SetAsync(EStorageKey.Password, "");
		await DeviceStorage.SetAsync(EStorageKey.JwtToken, "");
		await DeviceStorage.SetAsync(EStorageKey.JwtTokenExpiration, "");

		Application.Current.MainPage = new NavigationPage(new SignInPage());
    }
}