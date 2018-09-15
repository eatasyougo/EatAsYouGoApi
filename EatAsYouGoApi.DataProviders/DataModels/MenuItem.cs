using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    public class MenuItem
    {
        [Key]
        public long MenuItemId { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Photo { get; set; }

        public bool IsFlatDiscountVoucher { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<DealMenuItem> DealMenuItems { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}
