using System.Collections.Generic;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.Dtos;

namespace EatAsYouGoApi.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderDto> GetAllOrders();
        OrderDto GetOrderById(long orderId);
        OrderDto SaveOrder(OrderDto order);
        OrderDto ChargeOrder(string stripeToken, OrderDto order);
    }
}