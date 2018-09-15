using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    public class OrderDetails
    {
        [Key]
        public long OrderDetailsId { get; set; }
        [ForeignKey(nameof(Order))]
        public long OrderId { get; set; }
        [ForeignKey(nameof(Deal))]
        public long DealId { get; set; }
        public string VoucherCode { get; set; }
        public bool VoucherRedeemed { get; set; }
        public virtual Order Order { get; set; }
        public virtual Deal Deal { get; set; }
        public virtual ICollection<MenuItem> ItemsPurchased { get; set; }
    }
}