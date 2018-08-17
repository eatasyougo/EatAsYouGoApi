using System;
using AutoMapper;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.Dtos;

namespace EatAsYouGoApi.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<MenuItem, MenuItemDto>().ReverseMap();
            CreateMap<Restaurant, RestaurantDto>().ReverseMap();
            CreateMap<Deal, DealDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Group, GroupDto>().ReverseMap();
            CreateMap<Permission, PermissionDto>().ReverseMap();
        }
    }
}
