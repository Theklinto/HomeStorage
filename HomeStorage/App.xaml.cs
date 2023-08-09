namespace HomeStorage;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		MainPage = new UnauthenticatedAppShell();
	}
}
