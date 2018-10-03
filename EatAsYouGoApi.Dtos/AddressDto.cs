using System.Runtime.Serialization;

namespace EatAsYouGoApi.Dtos
{
    [DataContract(Name="Address")]
    public class AddressDto
    {
        [DataMember]
        public long AddressId { get; set; }

        [DataMember]
        public long RestaurantId { get; set; }

        [DataMember]
        public string AddLine1 { get; set; }

        [DataMember]
        public string AddLine2 { get; set; }

        [DataMember]
        public string AddLine3 { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string PostCode { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}