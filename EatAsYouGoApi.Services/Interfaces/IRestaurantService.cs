using System.Collections.Generic;
using EatAsYouGoApi.Dtos;

namespace EatAsYouGoApi.Services.Interfaces
{
    public interface IRestaurantService
    {
        IEnumerable<RestaurantDto> GetAllRestaurants();
        RestaurantDto GetRestaurantById(long restaurantId);
        IEnumerable<RestaurantDto> GetRestaurantsByCuisineType(string cuisineType);
        IEnumerable<RestaurantDto> GetRestaurantsByPostCode(string postCode);
        IEnumerable<RestaurantDto> GetRestaurantsByName(string restaurantName);
        RestaurantDto AddNewRestaurant(RestaurantDto restaurantDto);
        void RemoveRestaurant(long restaurantId);
        RestaurantDto UpdateRestaurant(RestaurantDto restaurantDto);
    }
}