using System;
using System.Web.Http;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;
using Swagger.Net.Swagger.Annotations;

namespace EatAsYouGoApi.Controllers
{
    public class MenuItemsController : BaseController
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemsController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [SwaggerDescription("Gets all menu items", "Gets all menu items")]
        [Route("api/menuItems")]
        public IHttpActionResult Get()
        {
            var menuItems = _menuItemService.GetAllMenuItems();
            return CreateResponse(menuItems);
        }

        [SwaggerDescription("Gets menu item by id", "Gets menu item by id")]
        [Route("api/menuItems/{menuItemId}")]
        public IHttpActionResult Get(long menuItemId)
        {
            var menuItem = _menuItemService.GetMenuItemById(menuItemId);
            return CreateResponse(menuItem);
        }

        [SwaggerDescription("Gets all menu items for a restaurant by restaurant id", "Gets all menu items for a restaurant by restaurant id")]
        [Route("api/restaurants/{restaurantId}/menuItems")]
        public IHttpActionResult GetAllMenuItemsForARestaurant(long restaurantId)
        {
            var menuItems = _menuItemService.GetAllMenuItemsForARestaurant(restaurantId);
            return CreateResponse(menuItems);
        }

        [SwaggerDescription("Gets all menu items by name", "Gets all menu items by name")]
        [Route("api/menuItems/name/{menuItemName}")]
        public IHttpActionResult GetAllMenuItemsByName(string menuItemName)
        {
            var menuItems = _menuItemService.GetAllMenuItemsByName(menuItemName);
            return CreateResponse(menuItems);
        }

        [SwaggerDescription("Adds new menu item", "Adds new menu item")]
        [Route("api/menuItems/add")]
        [HttpPost]
        public IHttpActionResult AddMenuItem([FromBody]MenuItemDto menuItemDto)
        {
            try
            {
                if (menuItemDto == null)
                    CreateErrorResponse($"Parameter {nameof(menuItemDto)} cannot be null");

                var addNewMenuItem = _menuItemService.AddNewMenuItem(menuItemDto);
                return CreateResponse(addNewMenuItem);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Removes a menu item", "Removes a menu item")]
        [Route("api/menuItems/delete/{menuItemId}")]
        [HttpPost]
        public IHttpActionResult RemoveMenuItem(long menuItemId)
        {
            try
            {
                if (menuItemId == 0)
                    CreateErrorResponse($"Parameter {nameof(menuItemId)} must be greater than 0");

                _menuItemService.RemoveMenuItem(menuItemId);
                return CreateEmptyResponse();
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Updates a menu item", "Updates a menu item")]
        [Route("api/menuItems/update")]
        [HttpPost]
        public IHttpActionResult UpdateMenuItem(MenuItemDto menuItemDto)
        {
            try
            {
                if (menuItemDto == null)
                    CreateErrorResponse($"Parameter {nameof(menuItemDto)} cannot be null");

                var updatedMenuItem = _menuItemService.UpdateMenuItem(menuItemDto);
                return CreateResponse(updatedMenuItem);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }
    }
}
