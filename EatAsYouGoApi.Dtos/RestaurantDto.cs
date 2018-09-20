using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EatAsYouGoApi.Dtos
{
    [DataContract(Name="Restaurant")]
    public class RestaurantDto
    {
        [DataMember]
        public long RestaurantId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string StripeAccountId { get; set; }

        [DataMember]
        public string CuisineType { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember(Name = "Addresses")]
        public virtual ICollection<AddressDto> Addresses { get; set; }

        [DataMember(Name = "MenuItems")]
        public virtual ICollection<MenuItemDto> MenuItems { get; set; }
    }
}