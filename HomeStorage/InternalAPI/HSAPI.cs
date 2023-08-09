using HomeStorage.DataAccess.AuthenticationModels;
using HomeStorage.Pages.Authentication;
using DeviceStorage = HomeStorage.SecureStorageExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AutoMapper;
using HomeStorage.ModelExtensions;
using HomeStorage.DataAccess.Entities;

namespace HomeStorage.InternalAPI
{
    public static partial class HSAPI
    {
        private static readonly HttpClient _client
#if DEBUG
            //If in debug accept all certificates inclusive selfsigned
            = new(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (o, cert, chain, errors) => true
            });
#else
            = new();
#endif
        private const string BaseAPIUrl = "https://10.0.2.2:44361/api/";

        /// <summary>
        /// Used to attach local stored jwt token to <see cref="HttpClient"/>. If token is expired, It will try to reauthenticate.
        /// If reauthenticaton failed, it will redirect to <see cref="SignInPage"/> and purge local storage authentication details.
        /// </summary>
        /// <param name="client"></param>
        private static async Task SetAuthenticationToken(this HttpClient client)
        {
            string jwtToken = string.Empty;

            //Check expiration date
            DateTime tokenExpiration = DeviceStorage.GetDate(EStorageKey.JwtTokenExpiration);
            if (tokenExpiration < DateTime.Now)
            {
                string email = DeviceStorage.GetString(EStorageKey.Email) ?? string.Empty;
                string password = DeviceStorage.GetString(EStorageKey.Password) ?? string.Empty;
                //Renew token
                JwtTokenModel tokenModel = await Authentication.Login(new()
                {
                    Email = email,
                    Password = password
                });

                //Reauthentication failed
                if (tokenModel.Token is null)
                {
                    Authentication.PurgeAuthenticationStorage();
                    Application.Current.MainPage = new UnauthenticatedAppShell();
                    return;
                }

                DeviceStorage.Set(EStorageKey.JwtToken, tokenModel.Token);
                DeviceStorage.Set(EStorageKey.JwtTokenExpiration, tokenModel.Expiration);
                jwtToken = tokenModel.Token;
            }

            if (string.IsNullOrWhiteSpace(jwtToken))
                jwtToken = DeviceStorage.GetString(EStorageKey.JwtToken) ?? string.Empty;

            client.DefaultRequestHeaders.Authorization = new("Bearer", jwtToken);
        }
    }
}
