using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HomeStorage.DataAccess.AuthenticationModels;
using Newtonsoft.Json;

namespace HomeStorage.InternalAPI
{
    public static partial class HSAPI
    {
        public static partial class Authentication
        {
            private static string _url { get; set; } = BaseAPIUrl + "authenticate/";
            public static async Task<JwtTokenModel> Login(LoginModel model)
            {
                HttpResponseMessage httpResponse = await _client.PostAsJsonAsync(_url + nameof(Login), model);
                if (httpResponse.IsSuccessStatusCode)
                {
                    JwtTokenModel jwtTokenModel = JsonConvert.DeserializeObject<JwtTokenModel>(await httpResponse.Content.ReadAsStringAsync());
                }

                return new();
            }

            public static async Task<ResponseModel> Register(RegisterModel model)
            {
                HttpResponseMessage httpResponse = await _client.PostAsJsonAsync(_url + nameof(Register), model);

                return JsonConvert.DeserializeObject<ResponseModel>(await httpResponse.Content.ReadAsStringAsync());
            }
        }
    }
}
