using AutoMapper;
using HomeStorage.DataAccess.Entities;
using HomeStorage.Logic.Models.Category;
using HomeStorage.Logic.Models.Location;
using HomeStorage.Logic.Models.Product;
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
            #region LocationMaps
            CreateMap<Location, LocationModel>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<Location, LocationListModel>()
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId));

            CreateMap<LocationUpdateModel, LocationModel>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<LocationUser, LocationUserModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.NormalizedUserName))
                .ForMember(dest => dest.LocationUserId, opt => opt.MapFrom(src => src.LocationUserId))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            #endregion

            CreateMap<Image, ImageModel>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.ImageBytes, opt => opt.MapFrom(src => src.ImageBytes));

            #region Categories
            CreateMap<Category, CategoryModel>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Category, CategoryNotationModel>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            #endregion

            #region Products
            CreateMap<Product, ProductModel>()
                .ReverseMap();
            #endregion


        }
    }
}
