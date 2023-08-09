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
        public ImageSource NewImageSource => ImageSource.FromStream(() => new MemoryStream(NewImage ?? Array.Empty<byte>()));
        //public ImageSource ImageSource => ImageSource.FromStream(() => new MemoryStream(Image ?? Array.Empty<byte>()));
    }
}
