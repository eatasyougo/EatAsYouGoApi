using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace EatAsYouGoApi.Dtos
{
    [DataContract(Name = "User")]
    public class UserDto
    {
        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataMember]
        [PasswordPropertyText(true)]
        public string Password { get; set; }

        [DataMember]
        [PasswordPropertyText(true)]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm password do not match")]
        public string ConfirmPassword { get; set; }

        [DataMember]
        [Phone]
        public string Mobile { get; set; }

        [DataMember]
        public string Landline { get; set; }

        [DataMember]
        public int LoginAttempts { get; set; }

        [DataMember]
        public bool AccountLocked { get; set; }

        [DataMember]
        public string PreferredLocation { get; set; }

        [DataMember]
        public long? RestaurantId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public virtual ICollection<GroupDto> Groups { get; set; }

    }
}
