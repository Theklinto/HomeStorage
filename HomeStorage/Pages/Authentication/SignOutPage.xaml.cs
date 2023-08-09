using HomeStorage.InternalAPI;
namespace HomeStorage.Pages.Authentication;

public partial class SignOutPage : ContentPage
{
	public SignOutPage()
	{
        HSAPI.Authentication.PurgeAuthenticationStorage();
        Application.Current.MainPage = new NavigationPage(new SignInPage());
    }
}