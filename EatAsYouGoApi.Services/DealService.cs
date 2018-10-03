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
    public class DealService : IDealService
    {
        private readonly IDealDataProvider _dealDataProvider;

        public DealService(IDealDataProvider dealDataProvider)
        {
            _dealDataProvider = dealDataProvider;
        }

        public IEnumerable<DealDto> GetAllDeals()
        {
            var allDeals = _dealDataProvider.GetAllDeals();
            return allDeals.Select(Mapper.Map<Deal, DealDto>).ToList();
        }

        public IEnumerable<DealDto> GetAllDealsForARestaurant(long restaurantId)
        {
            var allDeals = _dealDataProvider.GetAllDealsForARestaurant(restaurantId);
            return allDeals.Select(Mapper.Map<Deal, DealDto>).ToList();
        }

        public DealDto GetDealById(long dealId)
        {
            var deal = _dealDataProvider.GetDealById(dealId);
            return Mapper.Map<Deal, DealDto>(deal);
        }

        public DealDto AddNewDeal(DealDto dealDto)
        {
            var dealToAdd = Mapper.Map<DealDto, Deal>(dealDto);

            if (dealDto.MenuItems == null || dealDto.MenuItems.Count == 0)
                throw new InvalidOperationException("There must be at least one menu item added to the deal to create a new deal.");

            var menuItemsToAdd = dealDto.MenuItems.Select(Mapper.Map<MenuItemDto, MenuItem>).ToList();
            var newDeal = _dealDataProvider.AddNewDeal(dealToAdd, menuItemsToAdd);

            var newMenuItems = newDeal.DealMenuItems.Select(dmi => dmi.MenuItem).ToList();
            var newDealDto = Mapper.Map<Deal, DealDto>(newDeal);
            newDealDto.MenuItems = newMenuItems.Select(Mapper.Map<MenuItem, MenuItemDto>).ToList();

            return newDealDto;
        }

        public DealDto UpdateDeal(DealDto dealDto)
        {
            var deal = Mapper.Map<DealDto, Deal>(dealDto);

            if (dealDto.MenuItems == null || dealDto.MenuItems.Count == 0)
                throw new InvalidOperationException("There must be at least one menu item added to the deal to create a new deal.");

            var menuItems = dealDto.MenuItems.Select(Mapper.Map<MenuItemDto, MenuItem>).ToList();
            var updatedDeal = _dealDataProvider.UpdateDeal(deal, menuItems);

            var updatedMenuItems = updatedDeal.DealMenuItems.Select(ug => ug.MenuItem).ToList();
            var updatedDealDto = Mapper.Map<Deal, DealDto>(updatedDeal);
            updatedDealDto.MenuItems = updatedMenuItems.Select(Mapper.Map<MenuItem, MenuItemDto>).ToList();

            return updatedDealDto;
        }

        public void DeleteDeal(long dealId)
        {
            _dealDataProvider.DeleteDeal(dealId);
        }
    }
}