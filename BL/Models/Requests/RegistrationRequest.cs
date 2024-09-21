using System.ComponentModel.DataAnnotations;

namespace BL.Models.Requests
{
    public class RegistrationRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        [RegularExpression(Constants.PasswordRegex,
            ErrorMessage = Constants.WrongPasswordValidation)]
        public string? Password { get; set; }
    }
}
