using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EatAsYouGoApi.Dtos
{
    [DataContract(Name = "OrderDetails")]
    public class OrderDetailsDto
    {
        public long OrderId { get; set; }
        public long DealId { get; set; }
        public string VoucherCode { get; set; }
        public bool VoucherRedeemed { get; set; }
        public virtual ICollection<MenuItemDto> ItemsPurchased { get; set; }
    }
}