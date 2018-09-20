using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.DataLayer.DataProviders.Interfaces;

namespace EatAsYouGoApi.DataLayer.DataProviders
{
    public class DealDataProvider : IDealDataProvider
    {
        private readonly IDbContextFactory<EaygDbContext> _dbContextFactory;

        public DealDataProvider(IDbContextFactory<EaygDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IEnumerable<Deal> GetAllDeals()
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.Deals
                    .Include(d => d.DealMenuItems)
                    .Include(d => d.Restaurant)
                    .ToList();
            }
        }

        public IEnumerable<Deal> GetAllDealsForARestaurant(long restaurantId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.Deals
                    .Include(d => d.DealMenuItems)
                    .Include(d => d.Restaurant)
                    .Where(d => d.RestaurantId == restaurantId)
                    .ToList();
            }
        }

        public Deal GetDealById(long dealId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.Deals
                    .Include(d => d.DealMenuItems)
                    .Include(d => d.Restaurant)
                    .FirstOrDefault(d => d.DealId == dealId);
            }
        }

        public Deal AddNewDeal(Deal deal, ICollection<MenuItem> menuItems)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var dealAlreadyExists = dbContext.Deals.FirstOrDefault(d =>
                    d.RestaurantId == deal.RestaurantId &&
                    d.DealDate.Equals(deal.DealDate) &&
                    d.DealName.Equals(deal.DealName, StringComparison.OrdinalIgnoreCase));

                if(dealAlreadyExists != null)
                    throw new InvalidOperationException($"{deal.DealName} deal for date - {deal.DealDate:D} already exists for Restaurant - {deal.RestaurantId}");

                if (deal.DealMenuItems == null)
                    deal.DealMenuItems = new List<DealMenuItem>();

                foreach (var menuItem in menuItems)
                {
                    var dealMenuItem = new DealMenuItem { Deal = deal };

                    var menuItemFound = dbContext.MenuItems.FirstOrDefault(mi =>
                        mi.RestaurantId == menuItem.RestaurantId &&
                        mi.Name.Trim().Equals(menuItem.Name.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (menuItemFound == null)
                        dbContext.MenuItems.Add(menuItem);
                    else
                        menuItem.MenuItemId = menuItemFound.MenuItemId;

                    dealMenuItem.MenuItem = menuItem;
                    deal.DealMenuItems.Add(dealMenuItem);
                }

                dbContext.Deals.Add(deal);
                dbContext.SaveChanges();

                return deal;
            }
        }

        public Deal UpdateDeal(Deal deal, ICollection<MenuItem> menuItems)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var anotherDealAlreadyExists = dbContext.Deals.FirstOrDefault(d =>
                    d.DealId != deal.DealId && // if its a different deal but same settings then throw error
                    d.RestaurantId == deal.RestaurantId &&
                    d.DealDate.Equals(deal.DealDate) &&
                    d.DealName.Equals(deal.DealName, StringComparison.OrdinalIgnoreCase));

                if (anotherDealAlreadyExists != null)
                    throw new InvalidOperationException($"{deal.DealName} deal for date - {deal.DealDate:D} already exists for Restaurant - {deal.RestaurantId}");

                // When updating deal and menuitems in many-to-many relationship, we need to save deal and menuitem first to get the Ids
                // Then add the DealMenuItem link and save it to the database
                var dealToUpdate = dbContext.Deals
                    .Include(d => d.Restaurant)
                    .Include(d => d.DealMenuItems)
                    .FirstOrDefault(d => d.DealId == deal.DealId);

                if (dealToUpdate == null)
                        throw new DataException($"Deal with id - {deal.DealId} not found");

                    // Clear old menuitems and add new menuitems
                    if (menuItems != null && menuItems.Any())
                    {
                        dealToUpdate.DealMenuItems.Clear();
                        foreach (var menuItem in menuItems)
                        {
                            var menuItemFound = dbContext.MenuItems.FirstOrDefault(item =>
                                item.RestaurantId == menuItem.RestaurantId &&
                                item.Name.Trim().Equals(menuItem.Name.Trim(), StringComparison.OrdinalIgnoreCase));

                            if (menuItemFound == null)
                                dbContext.MenuItems.Add(menuItem);
                            else
                                menuItem.MenuItemId = menuItemFound.MenuItemId;
                        }
                    }

                    // add deal
                    dbContext.Entry(dealToUpdate).CurrentValues.SetValues(deal);

                    // save deal and menuItems first to generate ids
                    dbContext.SaveChanges();

                    // now add the link between the deal and the menuItems
                    if (menuItems == null || !menuItems.Any())
                        return dealToUpdate;

                    foreach (var menuItem in menuItems)
                    {
                        dealToUpdate.DealMenuItems.Add(
                            new DealMenuItem
                            {
                                MenuItemId = menuItem.MenuItemId,
                                DealId = deal.DealId
                            });
                    }

                    // finally save the link in the linking table
                    dbContext.SaveChanges();

                    return dealToUpdate;
            }
        }

        public void DeleteDeal(long dealId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var dealToDelete = dbContext.Deals
                    .Include(d => d.DealMenuItems)
                    .Include(d => d.Restaurant)
                    .FirstOrDefault(d => d.DealId == dealId);

                if(dealToDelete == null)
                    throw new InvalidOperationException($"Deal with Id - {dealId} cannot be found");

                if (dealToDelete.DealMenuItems != null)
                {
                    foreach (var dealMenuItem in dealToDelete.DealMenuItems)
                    {
                        dbContext.DealMenuItems.Remove(dealMenuItem);
                    }
                }

                dbContext.Deals.Remove(dealToDelete);
                dbContext.SaveChanges();
            }
        }
    }
}