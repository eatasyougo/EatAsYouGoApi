using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EatAsYouGoApi.Dtos
{
    public class LoginRequestDto
    {
        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [PasswordPropertyText]
        public string  Password { get; set; }
    }
}