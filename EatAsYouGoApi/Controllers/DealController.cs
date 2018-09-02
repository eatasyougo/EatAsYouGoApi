using System;
using System.Web.Http;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;
using Swagger.Net.Swagger.Annotations;

namespace EatAsYouGoApi.Controllers
{
    [Authorize]
    public class DealController : BaseController
    {
        private readonly IDealService _dealService;

        public DealController(IDealService dealService)
        {
            _dealService = dealService;
        }

        [SwaggerDescription("Gets all deals for all restaurants.", "Gets all deals for all restaurants")]
        [Route("api/deals")]
        [AllowAnonymous]
        public IHttpActionResult GetAllDeals()
        {
            var allDeals = _dealService.GetAllDeals();
            return CreateResponse(allDeals);
        }

        [SwaggerDescription("Gets all deals for a restaurant.", "Gets all deals for a restaurant")]
        [Route("api/restaurants/{restaurantId}/deals")]
        [AllowAnonymous]
        public IHttpActionResult GetAllDealsForARestaurant(long restaurantId)
        {
            var allDeals = _dealService.GetAllDealsForARestaurant(restaurantId);
            return CreateResponse(allDeals);
        }

        [SwaggerDescription("Gets deal by deal id.", "Gets deal by deal id")]
        [Route("api/deals/{dealId}")]
        [AllowAnonymous]
        public IHttpActionResult GetDealById(long dealId)
        {
            var deal = _dealService.GetDealById(dealId);
            return CreateResponse(deal);
        }

        [SwaggerDescription("Adds new deal", "Adds new deal")]
        [Route("api/deals/add")]
        [HttpPost]
        public IHttpActionResult AddNewDeal(DealDto deal)
        {
            try
            {
                if (deal == null)
                    CreateErrorResponse($"Parameter {nameof(deal)} cannot be null");

                var newDeal = _dealService.AddNewDeal(deal);
                return CreateResponse(newDeal);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Updates a menu item", "Updates a menu item")]
        [Route("api/deals/update")]
        [HttpPost]
        public IHttpActionResult UpdateDeal(DealDto deal)
        {
            var updatedDeal = _dealService.UpdateDeal(deal);
            return CreateResponse(updatedDeal);
        }

        [SwaggerDescription("Removes a deal", "Removes a deal")]
        [Route("api/deals/delete/{dealId}")]
        [HttpPost]
        public IHttpActionResult DeleteDeal(long dealId)
        {
            try
            {
                if (dealId == 0)
                    CreateErrorResponse($"Parameter {nameof(dealId)} must be greater than 0");

                _dealService.DeleteDeal(dealId);
                return CreateEmptyResponse();
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

    }
}
