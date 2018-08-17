using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    public class GroupPermission
    {
        [Key, Column(Order = 1)]
        public int GroupId { get; set; }

        [Key, Column(Order = 2)]
        public int PermissionId { get; set; }

        public virtual Group Group { get; set; }

        public virtual Permission Permission { get; set; }
    }
}