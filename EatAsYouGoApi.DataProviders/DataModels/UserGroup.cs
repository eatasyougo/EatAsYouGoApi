using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    public class UserGroup
    {
        [Key, Column(Order = 1)]
        public long UserId { get; set; }

        [Key, Column(Order = 2)]
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        public virtual User User { get; set; }
    }
}