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
            string tokenExpiration = await DeviceStorage.GetAsync(EStorageKey.JwtTokenExpiration);
            if (DateTime.TryParse(tokenExpiration, out DateTime tokenExpirationDate) && tokenExpirationDate < DateTime.Now)
            {
                string email = await DeviceStorage.GetAsync(EStorageKey.Email);
                string password = await DeviceStorage.GetAsync(EStorageKey.Password);
                //Renew token
                JwtTokenModel tokenModel = await Authentication.Login(new()
                {
                    Email = email,
                    Password = password
                });

                //Reauthentication failed
                if (tokenModel.Token is null)
                {
                    await Authentication.PurgeAuthenticationStorage();
                    Application.Current.MainPage = new NavigationPage(new SignInPage());
                    return;
                }

                await DeviceStorage.SetAsync(EStorageKey.JwtToken, tokenModel.Token);
                await DeviceStorage.SetAsync(EStorageKey.JwtTokenExpiration, tokenModel.Expiration);
                jwtToken = tokenModel.Token;
            }

            if (string.IsNullOrWhiteSpace(jwtToken))
                jwtToken = await DeviceStorage.GetAsync(EStorageKey.JwtToken);

            client.DefaultRequestHeaders.Authorization = new("Bearer", jwtToken);
        }
    }
}
