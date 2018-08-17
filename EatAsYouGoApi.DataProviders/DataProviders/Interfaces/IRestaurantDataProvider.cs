using System.Collections.Generic;
using EatAsYouGoApi.DataLayer.DataModels;

namespace EatAsYouGoApi.DataLayer.DataProviders.Interfaces
{
    public interface IRestaurantDataProvider
    {
        IEnumerable<Restaurant> GetAllRestaurants();

        Restaurant GetRestaurantById(long restaurantId);

        IEnumerable<Restaurant> GetRestaurantsByCuisineType(string cuisineType);

        IEnumerable<Restaurant> GetRestaurantsByPostCode(string postCode);

        IEnumerable<Restaurant> GetRestaurantsByName(string restaurantName);

        Restaurant AddNewRestaurant(Restaurant restaurant);

        void RemoveRestaurant(long restaurantId);

        Restaurant UpdateRestaurant(Restaurant restaurant);
    }
}