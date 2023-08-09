using HomeStorage.InternalAPI;

namespace HomeStorage;

public partial class UnauthenticatedAppShell : Shell
{
	public UnauthenticatedAppShell()
	{
		//Register all routes
		foreach (KeyValuePair<Type, string> route in HSAPI.Routing.UnauthenticatedPages)
			Routing.RegisterRoute(route.Value, route.Key);
		InitializeComponent();
	}
}