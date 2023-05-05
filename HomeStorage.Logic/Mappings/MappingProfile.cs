using AutoMapper;
using HomeStorage.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Location, LocationModel>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.LocationUsers, opt => opt.MapFrom(src => src.LocationUsers));

            CreateMap<LocationUser, LocationUserModel>()
                .ForMember(dest => dest.LocationUserId, opt => opt.MapFrom(src => src.LocationUserId))
                .ForMember(dest => dest.IsLocationOwner, opt => opt.MapFrom(src => src.IsLocationOwner))
                .ForMember(dest => dest.IsLoactionAdmin, opt => opt.MapFrom(src => src.IsLoactionAdmin))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId));

            CreateMap<Image, ImageModel>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId));


        }
    }
}
