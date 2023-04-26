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

		ResultText.IsVisible = true;
		ResultText.Text = response.Message;
		ResultText.TextColor = response.Success ? HSAPI.Colors.Success : HSAPI.Colors.Error;
    }
}