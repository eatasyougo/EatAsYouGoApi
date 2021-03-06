﻿using System;
using System.Web.Http;
using EatAsYouGoApi.Authentication;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;
using Swagger.Net.Swagger.Annotations;

namespace EatAsYouGoApi.Controllers
{
    public class DealController : BaseController
    {
        private readonly IDealService _dealService;

        public DealController(IDealService dealService, ILogService logService)
            : base(logService)
        {
            _dealService = dealService;
        }

        [SwaggerDescription("Gets all deals for all restaurants.", "Gets all deals for all restaurants")]
        [Route("api/deals")]
        public IHttpActionResult GetAllDeals()
        {
            var allDeals = _dealService.GetAllDeals();
            return CreateResponse(allDeals);
        }

        [SwaggerDescription("Gets all deals for a restaurant.", "Gets all deals for a restaurant")]
        [Route("api/restaurants/{restaurantId}/deals")]
        public IHttpActionResult GetAllDealsForARestaurant(long restaurantId)
        {
            var allDeals = _dealService.GetAllDealsForARestaurant(restaurantId);
            return CreateResponse(allDeals);
        }

        [SwaggerDescription("Gets deal by deal id.", "Gets deal by deal id")]
        [Route("api/deals/{dealId}")]
        public IHttpActionResult GetDealById(long dealId)
        {
            var deal = _dealService.GetDealById(dealId);
            return CreateResponse(deal);
        }

        [SwaggerDescription("Adds new deal", "Adds new deal")]
        [Route("api/deals/add")]
        [HttpPost]
        [AuthorizeGroups(Groups = "SiteAdministrators,RestaurantUsers")]
        public IHttpActionResult AddNewDeal(DealDto deal)
        {
            try
            {
                if (deal == null)
                    return CreateErrorResponse($"Parameter {nameof(deal)} cannot be null");

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
        [AuthorizeGroups(Groups = "SiteAdministrators,RestaurantUsers")]
        public IHttpActionResult UpdateDeal(DealDto deal)
        {
            try
            {
                var updatedDeal = _dealService.UpdateDeal(deal);
                return CreateResponse(updatedDeal);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Removes a deal", "Removes a deal")]
        [Route("api/deals/delete/{dealId}")]
        [HttpPost]
        [AuthorizeGroups(Groups = "SiteAdministrators,RestaurantUsers")]
        public IHttpActionResult DeleteDeal(long dealId)
        {
            try
            {
                if (dealId == 0)
                    return CreateErrorResponse($"Parameter {nameof(dealId)} must be greater than 0");

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
