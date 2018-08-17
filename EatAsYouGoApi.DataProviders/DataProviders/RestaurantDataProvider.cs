using System.Data.Entity;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.DataLayer.DataProviders.Interfaces;

namespace EatAsYouGoApi.DataLayer.DataProviders
{
    public class RestaurantDataProvider : IRestaurantDataProvider
    {
        private readonly IDbContextFactory<EaygDbContext> _dbContextFactory;

        public RestaurantDataProvider(IDbContextFactory<EaygDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IEnumerable<Restaurant> GetAllRestaurants()
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var restaurants = dbContext.Restaurants
                    .Include(nameof(Restaurant.Addresses))
                    .Include(nameof(Restaurant.MenuItems))
                    .ToList();

                return restaurants;
            }
        }

        public Restaurant GetRestaurantById(long restaurantId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.Restaurants
                    .Include(nameof(Restaurant.Addresses))
                    .Include(nameof(Restaurant.MenuItems))
                    .FirstOrDefault(r => r.RestaurantId == restaurantId);
            }
        }

        public IEnumerable<Restaurant> GetRestaurantsByCuisineType(string cuisineType)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.Restaurants
                    .Include(nameof(Restaurant.Addresses)) 
                    .Include(nameof(Restaurant.MenuItems))
                    .Where(restaurant =>
                        restaurant.CuisineType != null &&
                        restaurant.CuisineType.Trim().ToLower().Contains(cuisineType.Trim().ToLower()))
                    .ToList();
            }
        }

        public IEnumerable<Restaurant> GetRestaurantsByPostCode(string postCode)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.Restaurants
                    .Include(nameof(Restaurant.Addresses))
                    .Include(nameof(Restaurant.MenuItems))
                    .Where(restaurant => restaurant.Addresses.Any(
                        address => address.PostCode != null &&
                                   address.PostCode.Trim().ToLower().Contains(postCode.Trim().ToLower())))
                    .ToList();
            }
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string restaurantName)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.Restaurants
                    .Include(nameof(Restaurant.Addresses))
                    .Include(nameof(Restaurant.MenuItems))
                    .Where(restaurant => restaurant.Name.Trim().ToLower().Contains(restaurantName.ToLower().Trim()))
                    .ToList();
            }
        }

        public Restaurant AddNewRestaurant(Restaurant restaurant)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                dbContext.Restaurants.Add(restaurant);
                dbContext.SaveChanges();
                return restaurant;
            }
        }

        public void RemoveRestaurant(long restaurantId)
        {
            using (var dbcontext = _dbContextFactory.Create())
            {
                var restaurant = dbcontext.Restaurants.FirstOrDefault(r => r.RestaurantId == restaurantId);

                if(restaurant== null)
                    throw new DataException($"Restaurant with restaurant id - {restaurantId} not found");

                restaurant.IsActive = false;

                dbcontext.SaveChanges();
            }
        }

        public Restaurant UpdateRestaurant(Restaurant restaurant)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var restaurantToUpdate = dbContext.Restaurants
                    .Include(r => r.Addresses)
                    .Include(r => r.MenuItems)
                    .FirstOrDefault(r => r.RestaurantId == restaurant.RestaurantId);

                if (restaurantToUpdate == null)
                    throw new DataException($"Restaurant with restaurant id - {restaurant.RestaurantId} not found");

                foreach (var addressToUpdate in restaurantToUpdate.Addresses)
                {
                    var address = restaurant.Addresses.FirstOrDefault(a => a.AddressId == addressToUpdate.AddressId);
                    if (address != null)
                        dbContext.Entry(addressToUpdate).CurrentValues.SetValues(address);
                }

                foreach (var menuItemToUpdate in restaurantToUpdate.MenuItems)
                {
                    var menuItem = restaurant.MenuItems.FirstOrDefault(mi => mi.MenuItemId == menuItemToUpdate.MenuItemId);
                    if (menuItem != null)
                        dbContext.Entry(menuItemToUpdate).CurrentValues.SetValues(menuItem);
                }

                dbContext.Entry(restaurantToUpdate).CurrentValues.SetValues(restaurant);

                dbContext.SaveChanges();
                return restaurantToUpdate;
            }
        }
    }
}
