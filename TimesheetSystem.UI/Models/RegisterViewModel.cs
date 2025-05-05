using System.ComponentModel.DataAnnotations;

namespace TimesheetSystem.UI.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}
