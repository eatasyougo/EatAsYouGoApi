using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EatAsYouGoApi.DataLayer.DataModels
{
    public class User
    {
        [Key]
        public long UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [PasswordPropertyText(true)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Landline { get; set; }

        public int LoginAttempts { get; set; }

        public bool AccountLocked { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public long? RestaurantId { get; set; }

        public string PreferredLocation { get; set; }

        public string RefreshToken { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
