using System.Collections.Generic;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.Dtos;

namespace EatAsYouGoApi.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderDto> GetAllOrders();
        IEnumerable<OrderDto> GetOrderById(int orderId);
        OrderDto ChargeOrder(string stripeToken, Order order);
        OrderDto SaveOrder(OrderDto order);
    }
}