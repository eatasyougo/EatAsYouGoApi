using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    public class Address
    {
        [Key]
        public long AddressId { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        [Required]
        public string AddLine1 { get; set; }

        public string AddLine2 { get; set; }

        public string AddLine3 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string PostCode { get; set; }

        public bool IsActive { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}
