using System.Collections.Generic;
using EatAsYouGoApi.DataLayer.DataModels;

namespace EatAsYouGoApi.DataLayer.DataProviders
{
    public interface IOrderDataProvider
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int orderId);
        Order SaveOrder(Order order);
        Order ChargeOrder(string stripeToken, Order order);
    }
}