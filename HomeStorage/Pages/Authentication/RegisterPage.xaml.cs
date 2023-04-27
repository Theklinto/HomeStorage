using HomeStorage.DataAccess.AuthenticationModels;
using HomeStorage.InternalAPI;

namespace HomeStorage.Pages.Authentication;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    private async void RegisterClick(object sender, EventArgs e)
    {
		ResponseModel response = await HSAPI.Authentication.Register(new()
		{
			Email = Email.Text,
			Password = Password.Text,
			Username = Username.Text,
		});

		string alertTitle = response.Success ? "Bruger oprettet!" : "Bruger ikke oprettet!";
		await DisplayAlert(alertTitle, response.Message, "Ok");

		if(response.Success)
			await Shell.Current.GoToAsync(HSAPI.Routing.Pages[typeof(SignInPage)]);
    }
}