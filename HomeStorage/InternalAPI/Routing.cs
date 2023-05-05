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
                //Base
                {typeof(DefaultPage), "default" },

                //Authentication
                {typeof(Pages.Authentication.SignInPage), $"{nameof(Authentication)}/login" },
                {typeof(Pages.Authentication.RegisterPage), $"{nameof(Authentication)}/register" },
                {typeof(Pages.Authentication.SignOutPage), $"{nameof(Authentication)}/logout" },

                //Location
                {typeof(Pages.Locations.LocationListPage), $"{nameof(Location)}/list" },
                {typeof(Pages.Locations.LocationUpdatePage), $"{nameof(Location)}/location" },
            };
        }
    }
}
