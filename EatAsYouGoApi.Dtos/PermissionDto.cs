using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EatAsYouGoApi.Dtos
{
    [DataContract(Name = "Permission")]
    public class PermissionDto
    {
        [DataMember]
        public int PermissionId { get; set; }

        [DataMember]
        public string PermissionType { get; set; }

        [DataMember]
        public string PermissionDescription { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public virtual ICollection<GroupDto> Groups { get; set; }
    }
}