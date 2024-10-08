﻿using System.ComponentModel.DataAnnotations;

namespace BL.Models.DTO
{
    public class LoginDto
    {
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
