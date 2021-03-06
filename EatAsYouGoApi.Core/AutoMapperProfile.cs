﻿using AutoMapper;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services;

namespace EatAsYouGoApi.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            var securityService = new SecurityService();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<MenuItem, MenuItemDto>().ReverseMap();
            CreateMap<Restaurant, RestaurantDto>().ReverseMap();
            CreateMap<Deal, DealDto>().ReverseMap();
            CreateMap<User, UserDto>()
                //.ForMember(userDto => userDto.Password, opt => opt.MapFrom(user => securityService.DecryptString(user.Password)))
                .ReverseMap();

            CreateMap<Group, GroupDto>().ReverseMap();
            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
        }
    }
}
