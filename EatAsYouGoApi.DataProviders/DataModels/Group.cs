using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        
        [Required]
        public string GroupName { get; set; }

        public string GroupDescription { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }

        public virtual ICollection<GroupPermission> GroupPermissions { get; set; }
    }
}