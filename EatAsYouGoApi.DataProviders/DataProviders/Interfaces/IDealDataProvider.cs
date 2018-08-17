using System.Collections.Generic;
using EatAsYouGoApi.DataLayer.DataModels;

namespace EatAsYouGoApi.DataLayer.DataProviders.Interfaces
{
    public interface IDealDataProvider
    {
        IEnumerable<Deal> GetAllDeals();

        IEnumerable<Deal> GetAllDealsForARestaurant(long restaurantId);

        Deal GetDealById(long dealId);

        Deal AddNewDeal(Deal deal, ICollection<MenuItem> menuItems);

        Deal UpdateDeal(Deal deal, ICollection<MenuItem> menuItems);

        void DeleteDeal(long dealId);
    }
}
