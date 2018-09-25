using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.DataLayer.DataProviders;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;
using Stripe;

namespace EatAsYouGoApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDataProvider _orderDataProvider;

        public OrderService(IOrderDataProvider orderDataProvider)
        {
            _orderDataProvider = orderDataProvider;
        }

        public IEnumerable<OrderDto> GetAllOrders()
        {
            return Mapper.Map<List<OrderDto>>(_orderDataProvider.GetAllOrders());
        }

        public OrderDto GetOrderById(long orderId)
        {
            return Mapper.Map<OrderDto>(_orderDataProvider.GetOrderById(orderId));
        }

        public OrderDto ChargeOrder(string stripeToken, OrderDto orderDto)
        {
            Order order = _orderDataProvider.GetOrderById(orderDto.OrderId);
            int amountToCharge = (int)(Math.Round(order.Amount, 2) * 100);
            int applicationFee = (int)(Math.Round(order.Amount, 1) * 10);
            var chargeOptions = new StripeChargeCreateOptions()
            {
                //required
                Amount = amountToCharge,
                Currency = "gbp",
                SourceTokenOrExistingSourceId = stripeToken,
                //optional
                Description = $"{order.User.Email} ordered {order.OrderDetails.SelectMany(x => x.ItemsPurchased).Count()} items from {order.Restaurant.Name}",
                ReceiptEmail = order.User.Email,
                //Add destination account and application fee to charge restaurant
                Destination = order.Restaurant.StripeAccountId,
                ApplicationFee = applicationFee
            };

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var chargeService = new StripeChargeService();
            var stripeCharge = chargeService.Create(chargeOptions);
            order.OrderStatus = OrderStatus.Paid;
            order.PaymentStatus = OrderStatus.Paid;
            order.PaymentToken = stripeCharge.Id;
            _orderDataProvider.SaveOrder(order);
            return Mapper.Map<OrderDto>(order);
        }

        public OrderDto SaveOrder(OrderDto order)
        {
            Order savedOrder = _orderDataProvider.SaveOrder(Mapper.Map<Order>(order));
            return Mapper.Map<OrderDto>(savedOrder);
        }
    }
}