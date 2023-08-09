using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.InternalAPI
{
    public static partial class HSAPI
    {
        public static partial class Routing
        {
            public static readonly Dictionary<Type, string> Pages = new()
            {
                //Location
                {typeof(Pages.Locations.LocationListPage), $"{nameof(Location)}/list" },
                {typeof(Pages.Locations.LocationUpdatePage), $"{nameof(Location)}/location" },
            };

            public static readonly Dictionary<Type, string> UnauthenticatedPages = new List<Type>()
            {
                typeof(Pages.Authentication.LandingPage),
                typeof(Pages.Authentication.SignInPage),
                typeof(Pages.Authentication.RegisterPage),
                typeof(Pages.Authentication.SignOutPage),
            }.ToDictionary(
                key => key,
                val => $"{nameof(Authentication)}/{val.Name}");
        }
    }
}
