using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    public class Restaurant
    {
        [Key]
        public long RestaurantId { get; set; }

        [Required]
        public string Name { get; set; }

        public string StripeAccountId { get; set; }

        public string CuisineType { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }

        public virtual ICollection<Deal> Deals { get; set; }
    }

    public class Commission
    {
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal CommissionPercentage { get; set; }
    }
}
