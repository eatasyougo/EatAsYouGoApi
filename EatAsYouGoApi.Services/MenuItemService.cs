using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.DataLayer.DataProviders.Interfaces;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;

namespace EatAsYouGoApi.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemDataProvider _menuItemDataProvider;

        public MenuItemService(IMenuItemDataProvider menuItemDataProvider)
        {
            _menuItemDataProvider = menuItemDataProvider;
        }

        public IEnumerable<MenuItemDto> GetAllMenuItems()
        {
            return _menuItemDataProvider.GetAllMenuItems()
                .Select(Mapper.Map<MenuItem, MenuItemDto>)
                .ToList();
        }

        public MenuItemDto GetMenuItemById(long menuItemId)
        {
            var menuItem = _menuItemDataProvider.GetMenuItemById(menuItemId);
            return Mapper.Map<MenuItem, MenuItemDto>(menuItem);
        }

        public IEnumerable<MenuItemDto> GetAllMenuItemsForARestaurant(long restaurantId)
        {
            return _menuItemDataProvider.GetAllMenuItemsForARestaurant(restaurantId)
                .Select(Mapper.Map<MenuItem, MenuItemDto>)
                .ToList();
        }

        public IEnumerable<MenuItemDto> GetAllMenuItemsByName(string menuItemName)
        {
            return _menuItemDataProvider.GetAllMenuItemsByName(menuItemName)
                .Select(Mapper.Map<MenuItem, MenuItemDto>)
                .ToList();
        }

        public MenuItemDto AddNewMenuItem(MenuItemDto menuItemDto)
        {
            var menuItemToAdd = Mapper.Map<MenuItemDto, MenuItem>(menuItemDto);
            var newMenuItem = _menuItemDataProvider.AddNewMenuItem(menuItemToAdd);
            return Mapper.Map<MenuItem, MenuItemDto>(newMenuItem);
        }

        public void RemoveMenuItem(long menuItemId)
        {
            _menuItemDataProvider.RemoveMenuItem(menuItemId);
        }

        public MenuItemDto UpdateMenuItem(MenuItemDto menuItemDto)
        {
            var menuItemToUpdate = Mapper.Map<MenuItemDto, MenuItem>(menuItemDto);
            var updatedMenuItem = _menuItemDataProvider.UpdateMenuItem(menuItemToUpdate);
            return Mapper.Map<MenuItem, MenuItemDto>(updatedMenuItem);
        }
    }
}