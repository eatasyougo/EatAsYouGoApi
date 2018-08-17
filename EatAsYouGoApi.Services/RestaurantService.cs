using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.DataLayer.DataProviders.Interfaces;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;

namespace EatAsYouGoApi.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantDataProvider _restaurantDataProvider;

        public RestaurantService(IRestaurantDataProvider restaurtDataProvider)
        {
            _restaurantDataProvider = restaurtDataProvider;
        }

        public IEnumerable<RestaurantDto> GetAllRestaurants()
        {
            var restaurants = _restaurantDataProvider.GetAllRestaurants().ToList();
            var restaurantDtos = restaurants.Select(Mapper.Map<Restaurant, RestaurantDto>).ToList();
            return restaurantDtos;
        }

        public RestaurantDto GetRestaurantById(long restaurantId)
        {
            var restaurant = _restaurantDataProvider.GetRestaurantById(restaurantId);
            return Mapper.Map<Restaurant, RestaurantDto>(restaurant);
        }

        public IEnumerable<RestaurantDto> GetRestaurantsByCuisineType(string cuisineType)
        {
            return _restaurantDataProvider.GetRestaurantsByCuisineType(cuisineType)
                .Select(Mapper.Map<Restaurant, RestaurantDto>)
                .ToList();
        }

        public IEnumerable<RestaurantDto> GetRestaurantsByPostCode(string postCode)
        {
            return _restaurantDataProvider.GetRestaurantsByPostCode(postCode)
                .Select(Mapper.Map<Restaurant, RestaurantDto>)
                .ToList();
        }

        public IEnumerable<RestaurantDto> GetRestaurantsByName(string restaurantName)
        {
            return _restaurantDataProvider.GetRestaurantsByName(restaurantName)
                .Select(Mapper.Map<Restaurant, RestaurantDto>)
                .ToList();
        }
        
        public RestaurantDto AddNewRestaurant(RestaurantDto restaurantDto)
        {
            var restaurant = Mapper.Map<RestaurantDto, Restaurant>(restaurantDto);
            var savedRestaurant = _restaurantDataProvider.AddNewRestaurant(restaurant);
            return Mapper.Map<Restaurant, RestaurantDto>(savedRestaurant);
        }

        public void RemoveRestaurant(long restaurantId)
        {
            _restaurantDataProvider.RemoveRestaurant(restaurantId);
        }

        public RestaurantDto UpdateRestaurant(RestaurantDto restaurantDto)
        {
            var restaurant = Mapper.Map<RestaurantDto, Restaurant>(restaurantDto);
            var updatedRestaurantDto = _restaurantDataProvider.UpdateRestaurant(restaurant);
            return Mapper.Map<Restaurant, RestaurantDto>(updatedRestaurantDto);
        }
    }
}