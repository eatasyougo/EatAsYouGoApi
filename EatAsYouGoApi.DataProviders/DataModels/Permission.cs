using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }

        [Required]
        public string PermissionType { get; set; }

        public string PermissionDescription { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<GroupPermission> GroupPermissions { get; set; }

    }
}