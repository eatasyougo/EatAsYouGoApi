using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EatAsYouGoApi.Dtos
{
    [DataContract(Name = "Deal")]
    public class DealDto
    {
        [DataMember]
        public long DealId { get; set; }

        [DataMember]
        public string DealName { get; set; }

        [DataMember]
        public DateTime DealDate { get; set; }

        [DataMember]
        public TimeSpan TimeFrom { get; set; }

        [DataMember]
        public TimeSpan TimeTo { get; set; }

        [DataMember]
        public TimeSpan? OrderByTime { get; set; }

        [DataMember]
        public int? MaxOrder { get; set; }

        [DataMember]
        public virtual ICollection<MenuItemDto> MenuItems { get; set; }
    }
}
