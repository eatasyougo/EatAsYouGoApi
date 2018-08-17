using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.DataLayer.DataProviders.Interfaces;

namespace EatAsYouGoApi.DataLayer.DataProviders
{
    public class MenuItemDataProvider : IMenuItemDataProvider
    {
        private readonly IDbContextFactory<EaygDbContext> _dbContextFactory;

        public MenuItemDataProvider(IDbContextFactory<EaygDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IEnumerable<MenuItem> GetAllMenuItems()
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.MenuItems.Include(nameof(MenuItem.Restaurant)).ToList();
            }
        }

        public MenuItem GetMenuItemById(long menuItemId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.MenuItems.Include(nameof(MenuItem.Restaurant))
                    .FirstOrDefault(mi => mi.MenuItemId == menuItemId);
            }
        }

        public IEnumerable<MenuItem> GetAllMenuItemsForARestaurant(long restaurantId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.MenuItems.Include(nameof(MenuItem.Restaurant))
                    .Where(menuItem => menuItem.RestaurantId == restaurantId)
                    .ToList();
            }
        }

        public IEnumerable<MenuItem> GetAllMenuItemsByName(string menuItemName)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.MenuItems.Include(nameof(MenuItem.Restaurant))
                    .Where(menuItem => menuItem.Name.ToLower().Contains(menuItemName.ToLower()))
                    .ToList();
            }
        }

        public MenuItem AddNewMenuItem(MenuItem menuItem)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                dbContext.MenuItems.Add(menuItem);
                dbContext.SaveChanges();
                return menuItem;
            }
        }

        public void RemoveMenuItem(long menuItemId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var menuItem = dbContext.MenuItems
                    .Include(m => m.DealMenuItems)
                    .FirstOrDefault(item => item.MenuItemId == menuItemId);

                if (menuItem == null)
                    throw new DataException($"MenuItem with id - {menuItemId} not found");

                //if (menuItem.DealMenuItems != null)
                //{
                //    foreach (var dealMenuItem in menuItem.DealMenuItems)
                //        dbContext.DealMenuItems.Remove(dealMenuItem);
                //}

                menuItem.IsActive = false;
                dbContext.SaveChanges();
            }
        }

        public MenuItem UpdateMenuItem(MenuItem menuItem)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var menuItemToUpdate = dbContext.MenuItems.Include(mi => mi.Restaurant)
                    .FirstOrDefault(item => item.MenuItemId == menuItem.MenuItemId);

                if (menuItemToUpdate == null)
                    throw new DataException($"Menu item with id - {menuItem.MenuItemId} not found");

                dbContext.Entry(menuItemToUpdate).CurrentValues.SetValues(menuItem);
                dbContext.SaveChanges();
                return menuItemToUpdate;
            }
        }
    }
}