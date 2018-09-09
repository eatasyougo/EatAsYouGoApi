using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    public class Order
    {
        [Key]
        public long OrderId { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentToken { get; set; }
        public decimal Amount { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderDealVoucher> OrderDealVouchers { get; set; }
    }

    public class OrderDealVoucher
    {
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

    public class OrderDealMenuItem
    {
        [ForeignKey(nameof(Order))]
        public long OrderId { get; set; }
        [ForeignKey(nameof(Deal))]
        public long DealId { get; set; }
        [ForeignKey(nameof(MenuItem))]
        public long MenuItemId { get; set; }
        public virtual Order Order { get; set; }
        public virtual Deal Deal { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }

    public class Deal
    {
        [Key]
        public long DealId { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        [Required]
        public string DealName { get; set; }

        [Required]
        public DateTime DealDate { get; set; }

        [Required]
        public TimeSpan TimeFrom { get; set; }

        [Required]
        public TimeSpan TimeTo { get; set; }
        
        public TimeSpan? OrderByTime { get; set; }

        public int? MaxOrder { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public virtual ICollection<DealMenuItem> DealMenuItems { get; set; }
    }
}
