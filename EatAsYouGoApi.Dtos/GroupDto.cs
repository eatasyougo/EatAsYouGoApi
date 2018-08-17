using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EatAsYouGoApi.Dtos
{
    [DataContract(Name="Group")]
    public class GroupDto
    {
        [DataMember]
        public int GroupId { get; set; }

        [DataMember]
        [Required]
        public string GroupName { get; set; }

        [DataMember]
        public string GroupDescription { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public virtual ICollection<PermissionDto> Permissions { get; set; }

        [DataMember]
        public virtual ICollection<UserDto> Users { get; set; }
    }
}