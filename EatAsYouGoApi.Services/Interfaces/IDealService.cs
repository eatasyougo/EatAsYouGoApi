using System.Collections.Generic;
using EatAsYouGoApi.Dtos;

namespace EatAsYouGoApi.Services.Interfaces
{
    public interface IDealService
    {
        IEnumerable<DealDto> GetAllDeals();

        IEnumerable<DealDto> GetAllDealsForARestaurant(long restaurantId);

        DealDto GetDealById(long dealId);

        DealDto AddNewDeal(DealDto dealDto);

        DealDto UpdateDeal(DealDto dealDto);

        void DeleteDeal(long dealId);
    }
}
