using System.Collections.Generic;
using EatAsYouGoApi.Dtos;

namespace EatAsYouGoApi.Services.Interfaces
{
    public interface IMenuItemService
    {
        IEnumerable<MenuItemDto> GetAllMenuItems();
        MenuItemDto GetMenuItemById(long menuItemId);
        IEnumerable<MenuItemDto> GetAllMenuItemsForARestaurant(long restaurantId);
        IEnumerable<MenuItemDto> GetAllMenuItemsByName(string menuItemName);
        MenuItemDto AddNewMenuItem(MenuItemDto menuItemDto);
        void RemoveMenuItem(long menuItemId);
        MenuItemDto UpdateMenuItem(MenuItemDto menuItemDto);
    }
}
