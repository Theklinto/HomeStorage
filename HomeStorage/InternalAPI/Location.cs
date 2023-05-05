using Newtonsoft.Json;
using HomeStorage.ModelExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace HomeStorage.InternalAPI
{
    public static partial class HSAPI
    {
        public static class Location
        {
            private static readonly string _url = BaseAPIUrl.TrimEnd('/') + "/location";

            public static async Task<IEnumerable<LocationModel>> GetAllLocationsAsync()
            {
                IEnumerable<LocationModel> locations = Enumerable.Empty<LocationModel>();
                    
                await _client.SetAuthenticationToken();
                HttpResponseMessage response = await _client.GetAsync(_url + "/GetAllLocations");
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    locations = JsonConvert.DeserializeObject<IEnumerable<LocationModel>>(responseContent);
                }
                
                return locations;
            }

            public static async Task<LocationModel> GetLocationByIdAsync(Guid locationId)
            {
                LocationModel location = new();

                if (locationId == Guid.Empty)
                    return location;

                await _client.SetAuthenticationToken();
                HttpResponseMessage response = await _client.GetAsync(_url + $"/GetLocation?locationid={locationId}");
                if(response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    location = JsonConvert.DeserializeObject<LocationModel>(responseContent);
                }

                return location;
            }

            public static async Task<ResponseModel> CreateNewLocationAsync(LocationModel location)
            {
                if (location.LocationId != Guid.Empty)
                    return new()
                    {
                        Message = "Der kan ikke oprettes en lokation med et eksisterende id"
                    };

                await _client.SetAuthenticationToken();

                //BaseConverter(location, out DataAccess.Entities.LocationModel locationModel);
                //DataAccess.Entities.LocationModel locationModel = _mapper.Map<DataAccess.Entities.LocationModel>(location);

                HttpResponseMessage response = await _client.PostAsJsonAsync(_url + "/CreateLocation", location);
                if(response.IsSuccessStatusCode)
                {
                    return new()
                    {
                        Message = "Lokation oprettet korrekt",
                        Success = true,
                    };
                }

                return new()
                {
                    Message = "Der skete en ukendt fejl"
                };
            }
        }
    }
}
