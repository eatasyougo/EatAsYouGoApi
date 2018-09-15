using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EatAsYouGoApi.Dtos
{
    [DataContract(Name = "Order")]
    public class OrderDto
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentToken { get; set; }
        public decimal Amount { get; set; }
        public virtual ICollection<OrderDetailsDto> OrderDealVouchers { get; set; }
    }
}