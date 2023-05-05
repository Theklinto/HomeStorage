using AutoMapper;
using HomeStorage.InternalAPI;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.ModelExtensions
{
    public class LocationModel : DataAccess.Entities.LocationModel
    {
        public Uri ImageUrl => HSAPI.Image.GetUri(ImageId.GetValueOrDefault());
        public ImageSource ImageSource => HSAPI.Image.GetSource(ImageId.GetValueOrDefault());
    }
}
