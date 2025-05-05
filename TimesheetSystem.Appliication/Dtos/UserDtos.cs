using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimesheetSystem.Appliication.Dtos
{
    public class UserDtos
    {
    }


    public class CreateUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }
    }


    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class LoginResultDto
    {
        public string Message { get; set; } = "";
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Token { get; set; } = "";
        public DateTime ExpiresOn { get; set; }
    }

    public class RegisterationDto
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
