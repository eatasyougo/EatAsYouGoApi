using System.Collections.Generic;
using EatAsYouGoApi.DataLayer.DataModels;

namespace EatAsYouGoApi.DataLayer.DataProviders.Interfaces
{
    public interface IMenuItemDataProvider
    {
        IEnumerable<MenuItem> GetAllMenuItems();

        MenuItem GetMenuItemById(long menuItemId);

        IEnumerable<MenuItem> GetAllMenuItemsForARestaurant(long restaurantId);

        IEnumerable<MenuItem> GetAllMenuItemsByName(string menuItemName);

        MenuItem AddNewMenuItem(MenuItem menuItem);

        void RemoveMenuItem(long menuItemId);

        MenuItem UpdateMenuItem(MenuItem menuItem);
    }
}