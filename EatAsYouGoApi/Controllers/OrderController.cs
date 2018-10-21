using System;
using System.Web.Http;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;
using Swagger.Net.Swagger.Annotations;

namespace EatAsYouGoApi.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService, ILogService logService) 
            : base(logService)
        {
            _orderService = orderService;
        }

        [SwaggerDescription("Gets all orders", "Gets all orders")]
        [Route("api/orders/get")]
        public IHttpActionResult Get()
        {
            try
            {
                var orders = _orderService.GetAllOrders();
                return CreateResponse(orders);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Get order by id", "Get order by id")]
        [Route("api/orders/get/{id}")]
        public IHttpActionResult GetOrderById(long id)
        {
            try
            {
                var order = _orderService.GetOrderById(id);
                return CreateResponse(order);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

        [SwaggerDescription("Charge order", "Charge order")]
        [Route("api/orders/change/")]
        [HttpPost]
        public IHttpActionResult ChargeOrder(string stripeToken, OrderDto order)
        {
            try
            {
                var savedOrder = _orderService.ChargeOrder(stripeToken, order);
                return CreateResponse(savedOrder);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }


        [SwaggerDescription("Save order", "Save order")]
        [Route("api/orders/save/")]
        [HttpPost]
        public IHttpActionResult SaveOrder(OrderDto order)
        {
            try
            {
                var savedOrder = _orderService.SaveOrder(order);
                return CreateResponse(savedOrder);
            }
            catch (Exception exception)
            {
                LogError(this.GetType(), exception.Message);
                return CreateErrorResponse(exception.Message, exception);
            }
        }

    }
}