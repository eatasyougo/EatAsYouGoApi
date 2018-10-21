using System;
using System.Web.Http;
using EatAsYouGoApi.Authentication;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;
using Swagger.Net.Swagger.Annotations;

namespace EatAsYouGoApi.Controllers
{
    public class RestaurantController : BaseController
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService, ILogService logService)
            : base(logService)
        {
            _restaurantService = restaurantService; 
        }

        [SwaggerDescription("Gets all restaurants", "Gets all restaurants")]
        [Route("api/restaurants/get")]
        public IHttpActionResult Get()
        {
            try
            {
                var restaurants = _restaurantService.GetAllRestaurants();
                return CreateResponse(restaurants);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Gets restaurant by id", "Gets restaurant by id")]
        [Route("api/restaurants/get/{id}")]
        public IHttpActionResult GetRestaurantById(long id)
        {
            try
            {
                var restaurant = _restaurantService.GetRestaurantById(id);
                return CreateResponse(restaurant);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Gets all restaurants by cuisine type", "Gets all restaurants by cuisine type")]
        [Route("api/restaurants/cuisineType/{cuisineType}")]
        public IHttpActionResult GetAllRestaurantsByCuisineType(string cuisineType)
        {
            try
            {
                var restaurants = _restaurantService.GetRestaurantsByCuisineType(cuisineType);
                return CreateResponse(restaurants);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Gets all restaurants by post code", "Gets all restaurants by post code")]
        [Route("api/restaurants/postCode/{postCode}")]
        public IHttpActionResult GetRestaurantByPostCode(string postCode)
        {
            try
            {
                var restaurants = _restaurantService.GetRestaurantsByPostCode(postCode);
                return CreateResponse(restaurants);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Gets all restaurants by name", "Gets all restaurants by name")]
        [Route("api/restaurants/name/{name}")]
        public IHttpActionResult GetRestaurantsByName(string name)
        {
            try
            {
                var restaurants = _restaurantService.GetRestaurantsByName(name);
                return CreateResponse(restaurants);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Adds new restaurant", "Adds new restaurant")]
        [Route("api/restaurants/add")]
        [HttpPost]
        [AuthorizeGroups(Groups = "SiteAdministrators")]
        public IHttpActionResult AddRestaurant([FromBody]RestaurantDto restaurantDto)
        {
            try
            {
                if (restaurantDto == null)
                    return CreateErrorResponse($"Parameter {nameof(restaurantDto)} cannot be null");

                var savedRestaurantDto = _restaurantService.AddNewRestaurant(restaurantDto);
                return CreateResponse(savedRestaurantDto);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Removes a restaurant", "Removes a restaurant")]
        [Route("api/restaurants/delete/{restaurantId}")]
        [HttpPost]
        [AuthorizeGroups(Groups = "SiteAdministrators")]
        public IHttpActionResult RemoveRestaurant(long restaurantId)
        {
            try
            {
                if (restaurantId == 0)
                    return CreateErrorResponse($"Parameter {nameof(restaurantId)} must be greater than 0");

                _restaurantService.RemoveRestaurant(restaurantId);
                return CreateEmptyResponse();
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Updates a restaurant", "Updates a restaurant")]
        [Route("api/restaurants/update")]
        [HttpPost]
        [AuthorizeGroups(Groups = "SiteAdministrators,RestaurantUsers")]
        public IHttpActionResult UpdateRestaurant(RestaurantDto restaurantDto)
        {
            try
            {
                if (restaurantDto == null)
                    return CreateErrorResponse($"Parameter {nameof(restaurantDto)} cannot be null");

                var updatedRestaurantDto = _restaurantService.UpdateRestaurant(restaurantDto);
                return CreateResponse(updatedRestaurantDto);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }
    }
}
