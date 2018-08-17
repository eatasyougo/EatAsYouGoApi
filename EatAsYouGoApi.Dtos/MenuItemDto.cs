using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EatAsYouGoApi.Dtos
{
    [DataContract(Name="MenuItem")]
    public class MenuItemDto
    {
        [DataMember]
        public long MenuItemId { get; set; }

        [DataMember]
        public long RestaurantId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public string Photo { get; set; }

        [DataMember]
        public bool IsFlatDiscountVoucher { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public virtual ICollection<DealDto> Deals { get; set; }
    }
}
