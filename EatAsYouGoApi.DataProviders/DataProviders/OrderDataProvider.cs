using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using EatAsYouGoApi.DataLayer.DataModels;
using Stripe;

namespace EatAsYouGoApi.DataLayer.DataProviders
{
    public class OrderDataProvider : IOrderDataProvider
    {
        private readonly IDbContextFactory<EaygDbContext> _dbContextFactory;

        public OrderDataProvider(IDbContextFactory<EaygDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.Orders
                    .Include(o => o.OrderDetails)
                    .Include(o => o.Restaurant)
                    .Include(o => o.User);

            }
        }

        public Order GetOrderById(long orderId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.Orders
                    .Include(o => o.OrderDetails)
                    .Include(o => o.Restaurant)
                    .Include(o => o.User)
                    .Single(o => o.OrderId == orderId);
            }
        }

        public Order SaveOrder(Order order)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                if(order.OrderId == 0)
                { 
                    dbContext.Orders.Add(order);
                }
                else
                {
                    dbContext.Orders.Attach(order);
                    dbContext.Entry(order).State = EntityState.Modified;
                }
                dbContext.SaveChanges();
                return order;
            }
        }

        public Order ChargeOrder(string stripeToken, Order order)
        {
            int amountToCharge = (int)(Math.Round(order.Amount, 2) * 100);
            int applicationFee = (int)(Math.Round(order.Amount, 1) * 10);
            var chargeOptions = new StripeChargeCreateOptions()
            {
                //required
                Amount = amountToCharge ,
                Currency = "gbp",
                SourceTokenOrExistingSourceId = stripeToken,
                //optional
                Description = $"{order.User.Email} ordered {order.OrderDetails.SelectMany(x => x.ItemsPurchased).Count()} items from {order.Restaurant.Name}",
                ReceiptEmail = order.User.Email,
                //Add destination account and application fee to charge restaurant
                Destination = "acct_1Cy41CFpT7CVA8J8",
                ApplicationFee = applicationFee
            };

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            var chargeService = new StripeChargeService();
            var stripeCharge = chargeService.Create(chargeOptions);
            order.OrderStatus = OrderStatus.Paid;
            order.PaymentStatus = OrderStatus.Paid;
            order.PaymentToken = stripeCharge.Id;
            return order;
        }
    }
}