using HomeStorage.InternalAPI;

namespace HomeStorage.Pages.Authentication;

public partial class SignInPage : ContentPage
{
	public SignInPage()
	{
		InitializeComponent();
	}

    private void SignInClicked(object sender, EventArgs e)
    {

    }

    private async void RegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(HSAPI.Routing.Pages[typeof(RegisterPage)]);
        return;
    }
}