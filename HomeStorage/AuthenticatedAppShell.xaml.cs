using HomeStorage.InternalAPI;

namespace HomeStorage;

public partial class AuthenticatedAppShell : Shell
{
	public AuthenticatedAppShell()
	{
		//Register all routes
		foreach(KeyValuePair<Type, string> route in HSAPI.Routing.Pages)
			Routing.RegisterRoute(route.Value, route.Key);


		InitializeComponent();
	}
}
